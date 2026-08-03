using Microsoft.EntityFrameworkCore;
using server.Enity;

namespace ERP_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Menu> Menus { get; set; }  // Added Menu DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique constraints for User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relationships for RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Menu Configuration
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.MenuId);

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Icon)
                    .HasMaxLength(50);

                entity.Property(e => e.Route)
                    .HasMaxLength(200);

                // Self-referencing relationship for parent-child menu hierarchy
                entity.HasOne(e => e.ParentMenu)
                    .WithMany(e => e.Children)
                    .HasForeignKey(e => e.ParentMenuId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes for better performance
                entity.HasIndex(e => e.ParentMenuId);
                entity.HasIndex(e => e.DisplayOrder);
                entity.HasIndex(e => e.IsActive);
            });
        }
    }
}