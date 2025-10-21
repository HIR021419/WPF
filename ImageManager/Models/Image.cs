namespace ImageManager.Models
{
    public class Image
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Path { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public virtual List<Tag> Tags { get; set; } = new();

        // Identification
        public string? Title { get; set; } = null!;
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }

        // Informations techniques
        public string? Camera { get; set; }
        public string? LensModel { get; set; }
        public string? Exposure { get; set; }
        public int? ISO { get; set; }
        public int? FocalLength { get; set; }
        public double? FocalNumber { get; set; }
        public string? EditingSoftware { get; set; }

        // Auteur et droits
        public string? Photographer { get; set; }
        public string? Copyright { get; set; }
        public string? License { get; set; }

        // Localisation
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Country { get; set; }
    }
}
