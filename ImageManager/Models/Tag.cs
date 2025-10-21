namespace ImageManager.Models
{
    public class Tag
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ValueId { get; set; }
        public TagValue Value { get; set; } = null!;
        public Guid ImageId { get; set; }
        public Image Image { get; set; } = null!;
    }
}
