using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageManager.Helpers
{
    static class ImageHelper
    {
        public static BitmapImage LoadThumbnail(string path, int decodePixelWidth = 200)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.UriSource = new Uri(path);
            bi.DecodePixelWidth = decodePixelWidth;
            bi.EndInit();
            bi.Freeze();
            return bi;
        }
    }
}
