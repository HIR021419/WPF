namespace ImageManager.Models
{
    public class TagValue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Value { get; set; } = null!;
    }
}
