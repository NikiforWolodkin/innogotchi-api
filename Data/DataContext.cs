using Microsoft.EntityFrameworkCore;
using innogotchi_api.Models;

namespace innogotchi_api.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Collaboration> Collaborations { get; set; }
        public DbSet<Innogotchi> Innogotchis { get; set; }
        public DbSet<FeedingAndQuenching> FeedingAndQuenchings { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collaboration>()
                .HasKey(c => new { c.FarmName, c.UserEmail });
            modelBuilder.Entity<Collaboration>()
                .HasOne(c => c.Farm)
                .WithMany(f => f.Collaborations)
                .HasForeignKey(c => c.FarmName);
            modelBuilder.Entity<Collaboration>()
                .HasOne(c => c.User)
                .WithMany(u => u.Collaborations)
                .HasForeignKey(c => c.UserEmail);
        }
    }
}
