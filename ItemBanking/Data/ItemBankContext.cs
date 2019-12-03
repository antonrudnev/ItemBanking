using ItemBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemBanking.Data
{
    public class ItemBankContext : DbContext
    {
        public ItemBankContext(DbContextOptions<ItemBankContext> options) : base(options)
        {

        }

        public DbSet<ItemBank> ItemBanks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Language> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemBank>().ToTable("ItemBank");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Item>().ToTable("Item");
            modelBuilder.Entity<Language>().ToTable("Language");
        }
    }
}