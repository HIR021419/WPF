using ImageManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Data
{
    public class ImageDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagValue> TagValues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=images.db");
    }
}
