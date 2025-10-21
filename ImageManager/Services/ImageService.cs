using ImageManager.Data;
using ImageManager.Models;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.EntityFrameworkCore;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;

namespace ImageManager.Services
{
    public class ImageService(ImageDbContext db)
    {
        public IEnumerable<Models.Image> LoadFromDirectory(string dir)
        {
            var exts = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
            DirectoryInfo d = new(dir);
            var files = d.GetFiles("*", SearchOption.TopDirectoryOnly)
                .Where(f => exts.Contains(f.Extension.ToLower()));

            var list = new List<Models.Image>();

            foreach (FileInfo file in files)
            {
                var img = new Models.Image
                {
                    Path = file.FullName,
                    FileName = file.Name,
                    Date = file.LastWriteTime
                };

                if (db.Images.Any(i => i.Path == img.Path)) continue;

                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(file.OpenRead());

                    var subIfd = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                    var ifd0 = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
                    var gps = directories.OfType<GpsDirectory>().FirstOrDefault();

                    if (subIfd != null)
                    {
                        img.Date = subIfd.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);
                        img.ISO = subIfd.GetInt32(ExifDirectoryBase.TagIsoEquivalent);
                        img.FocalLength = (int?)subIfd.GetRational(ExifDirectoryBase.TagFocalLength).ToDouble();
                        img.FocalNumber = subIfd.GetRational(ExifDirectoryBase.TagFNumber).ToDouble();
                        img.Exposure = subIfd.GetDescription(ExifDirectoryBase.TagExposureTime);
                        img.Camera = subIfd.GetDescription(ExifDirectoryBase.TagModel);
                        img.LensModel = subIfd.GetDescription(ExifDirectoryBase.TagLensModel);
                    }

                    if (ifd0 != null)
                    {
                        img.EditingSoftware = ifd0.GetDescription(ExifDirectoryBase.TagSoftware);
                        img.Photographer = ifd0.GetDescription(ExifDirectoryBase.TagArtist);
                        img.Title = ifd0.GetDescription(ExifDirectoryBase.TagImageDescription) ?? file.Name;
                        img.Copyright = ifd0.GetDescription(ExifDirectoryBase.TagCopyright);
                    }

                    if (gps != null)
                    {
                        var success = gps.TryGetGeoLocation(out GeoLocation location);
                        if (success)
                        {
                            img.Latitude = location.Latitude;
                            img.Longitude = location.Longitude;
                        }
                    }
                }
                catch
                {
                    // On ignore les erreurs de lecture des métadonnées
                }

                list.Add(img);
            }

            db.AddRange(list);
            db.SaveChanges();

            return list;
        }

        public IEnumerable<Models.Image> LoadFromDatabase()
        {
            var images = db.Images.Include(i => i.Tags).ToList();
            var toRemove = new List<Models.Image>();

            foreach (var img in images)
            {
                if (!File.Exists(img.Path)) toRemove.Add(img);
            }

            if (toRemove.Any())
            {
                db.Images.RemoveRange(toRemove);
                db.SaveChanges();
            }

            return images.Except(toRemove).ToList();
        }

        public void SaveImages(IEnumerable<Models.Image> images)
        {
            db.Images.UpdateRange(images);
            db.SaveChanges();
        }

        public bool RotateImage(Models.Image image, int degrees = 90)
        {
            if (!File.Exists(image.Path))
                return false;

            try
            {
                using (var img = SixLabors.ImageSharp.Image.Load(image.Path))
                {
                    img.Mutate(x => x.Rotate(degrees));
                    img.Save(image.Path);
                }

                image.Rotation = (image.Rotation + degrees) % 360;
                db.Images.Update(image);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error rotating {image.Path}: {ex.Message}");
                return false;
            }
        }
    }
}
