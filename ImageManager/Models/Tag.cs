namespace ImageManager.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;

        public Guid ImageId { get; set; }
        public Image Image { get; set; } = null!;

        public override string ToString() => Name;
    }
}
