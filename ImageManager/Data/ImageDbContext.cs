using ImageManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Data
{
    public class ImageDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=images.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>()
                .HasMany(i => i.Tags)
                .WithOne(t => t.Image)
                .HasForeignKey(t => t.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
                .HasIndex(t => new { t.ImageId, t.Name })
                .IsUnique();
        }
    }
}
