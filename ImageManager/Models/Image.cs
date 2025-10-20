using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManager.Models
{
    class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime DateTaken { get; set; }
        public long Size { get; set; }
        public string MimeType { get; set; }
        public double? Latitude { get; set; }     // GPS optional
        public double? Longitude { get; set; }
        public bool Archived { get; set; }
        public virtual List<Tag> Tags { get; set; } = new();
    }
}
