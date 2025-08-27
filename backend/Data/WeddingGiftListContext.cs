using Microsoft.EntityFrameworkCore;
using WeddingGiftList.Api.Models;

namespace WeddingGiftList.Api.Data;

public class WeddingGiftListContext : DbContext
{
    public WeddingGiftListContext(DbContextOptions<WeddingGiftListContext> options)
        : base(options)
    {
    }

    public DbSet<Gift> Gifts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Gift entity
        modelBuilder.Entity<Gift>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.TakenByGuestName).HasMaxLength(100);
            entity.Property(e => e.RowVersion).IsRowVersion();
        });

        // Seed data
        modelBuilder.Entity<Gift>().HasData(
            new Gift
            {
                Id = 1,
                Name = "Coffee Machine",
                Description = "A high-quality espresso machine for the perfect morning brew",
                ImageUrl = "https://images.unsplash.com/photo-1559056199-641a0ac8b55e?w=400&h=300&fit=crop"
            },
            new Gift
            {
                Id = 2,
                Name = "Kitchen Knife Set",
                Description = "Professional-grade knife set with wooden block",
                ImageUrl = "https://images.unsplash.com/photo-1594736797933-d0401ba2fe65?w=400&h=300&fit=crop"
            },
            new Gift
            {
                Id = 3,
                Name = "Silk Bed Sheets",
                Description = "Luxurious 100% silk bed sheets in ivory white",
                ImageUrl = "https://images.unsplash.com/photo-1505693416388-ac5ce068fe85?w=400&h=300&fit=crop"
            },
            new Gift
            {
                Id = 4,
                Name = "Wine Glasses Set",
                Description = "Crystal wine glasses set of 6 with elegant design",
                ImageUrl = "https://images.unsplash.com/photo-1553979459-d2229ba7433a?w=400&h=300&fit=crop"
            },
            new Gift
            {
                Id = 5,
                Name = "Cast Iron Cookware",
                Description = "Premium cast iron pan and dutch oven set",
                ImageUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400&h=300&fit=crop"
            },
            new Gift
            {
                Id = 6,
                Name = "Photo Album",
                Description = "Handcrafted leather photo album for wedding memories",
                ImageUrl = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?w=400&h=300&fit=crop"
            }
        );
    }
}
