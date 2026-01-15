using Microsoft.EntityFrameworkCore;
using server.Entities; 
namespace server.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _config;

        public DataContext(DbContextOptions<DataContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.OrignalPrice)
               .HasPrecision(18, 2);

                entity.Property(p => p.DiscountPercentage)
                .HasColumnType("decimal(5,2)")
                 .IsRequired(false);

                entity.Property(p => p.DiscountAmount)
                .HasColumnType("decimal(18,2)")
                 .IsRequired(false);

            }
       );
      


            modelBuilder.Entity<Product>()
                .HasOne(p => p.Thumbnail)
                .WithOne(i => i.Product)
                .HasForeignKey<Product>(p => p.ThumbnailId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Categories>()
                .HasOne(p => p.Image)
                .WithOne(i => i.Category)
                .HasForeignKey<Categories>(c => c.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Brand>()
     .HasOne(b => b.Image)       
     .WithOne(i => i.Brand)      
     .HasForeignKey<Brand>(b => b.ImageId)  
     .OnDelete(DeleteBehavior.SetNull);


            base.OnModelCreating(modelBuilder); 

        }
        public DbSet<Product> product { get; set; }
        public DbSet<Brand> brand { get; set; }
        public DbSet<Categories> categories { get; set; }
        public DbSet<ProductReview> productreviews { get; set; }
        public DbSet<Image> image { get; set; }

        public DbSet<WishListItems> WishListItems { get; set; }
        public DbSet<CartItems> CartItems { get; set; }


        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Cart> Cart { get; set; }


        public DbSet<User> User { get; set; }






    }
}
