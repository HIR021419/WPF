using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageManager.Models;
using ImageManager.Data;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace ImageManager.Services
{
    class ImageService(ImageDbContext db)
    {
        public IEnumerable<Image> LoadFromDirectory(string dir)
        {
            var exts = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
            var files = System.IO.Directory.EnumerateFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                                 .Where(f => exts.Contains(Path.GetExtension(f).ToLower()));
            var list = new List<Image>();
            foreach (var f in files)
            {
                var info = new FileInfo(f);
                var img = new Image
                {
                    Path = f,
                    FileName = info.Name,
                    Size = info.Length,
                    DateTaken = GetDateTaken(f) ?? info.LastWriteTime
                };
                var gps = GetGps(f);
                if (gps != null) { img.Latitude = gps.Value.lat; img.Longitude = gps.Value.lon; }
                list.Add(img);
            }
            return list;
        }

        static DateTime? GetDateTaken(string path)
        {
            try
            {
                var dirs = ImageMetadataReader.ReadMetadata(path);
                var subIfd = dirs.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                if (subIfd != null && subIfd.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dt))
                    return dt;
            }
            catch { }
            return null;
        }

        static (double lat, double lon)? GetGps(string path)
        {
            try
            {
                var dirs = ImageMetadataReader.ReadMetadata(path);
                var gps = dirs.OfType<GpsDirectory>().FirstOrDefault();
                if (gps != null && gps.TryGetGeoLocation(out var loc))
                    return (loc.Latitude, loc.Longitude);
            }
            catch { }
            return null;
        }

        public void SaveImages(IEnumerable<Image> images)
        {
            db.Images.AddRange(images);
            db.SaveChanges();
        }
    }
}
