using System.ComponentModel;

namespace ImageManager.Models
{
    public class Image : INotifyPropertyChanged
    {
        private string _path = null!;
        private string _fileName = null!;
        private DateTime? _date = null;
        private double _rotation = 0.0;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }


        public virtual List<Tag> Tags { get; set; } = new();
        public double Rotation
        {
            get => _rotation;
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    OnPropertyChanged(nameof(Rotation));
                }
            }
        }

        // Identification
        public string? Title { get; set; } = null!;
        public string? Subject { get; set; }
        public string? Description { get; set; }

        public DateTime? Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        // Informations techniques
        public string? Camera { get; set; }
        public string? LensModel { get; set; }
        public string? Exposure { get; set; }
        public int? ISO { get; set; }
        public int? FocalLength { get; set; }
        public double? FocalNumber { get; set; }
        public string? EditingSoftware { get; set; }
        public int? Size { get; set; }

        // Auteur et droits
        public string? Photographer { get; set; }
        public string? Copyright { get; set; }
        public string? License { get; set; }

        // Localisation
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Country { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
