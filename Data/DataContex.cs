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
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Country> Countries { get; set; } 

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

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("GETUTCDATE()");

                // Unique constraints
                entity.HasIndex(e => e.CountryName)
                    .IsUnique()
                    .HasDatabaseName("IX_Country_Name");

                entity.HasIndex(e => e.CountryCode)
                    .IsUnique()
                    .HasDatabaseName("IX_Country_Code");

                // Index for performance
                entity.HasIndex(e => e.IsActive)
                    .HasDatabaseName("IX_Country_IsActive");
            });
        }
    }
}