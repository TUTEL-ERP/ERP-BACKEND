using server.Entities;

namespace server.Dto
{



    public class ProductResDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal OrignalPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }

        public decimal NewPrice { get; set; }
        public bool IsOnDiscount { get; set; }


        public int StockQuantity { get; set; }
        public bool InStock { get; set; }

        public double AvearageRating { get; set; }
        public int TotalReviews { get; set; }

        public bool IsFeatured { get; set; }

        public CategoryResDto Category { get; set; }

        public BrandResDto Brand { get; set; }

        public ImageDtoRes? Thumbnail { get; set; }
    }

    public class CreateProductReq

        {
            public string Name { get; set; }
            public string Description { get; set; }
        public decimal OrignalPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }   // badge: -20%
        public int StockQuantity { get; set; }
        public double AvearageRating { get; set; }

        public bool IsFeatured { get; set; }


        public int CategoryId { get; set; }
            
        public int BrandId { get; set; }

        public IFormFile Thumbnail { get; set; }
    }

    public class CreateBrandReq
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }

    }
    public class CreateCategoryReq
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }

    }

    public class CatalogSpec
    {

        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; }
        public int[]? brandId { get; set; }
        public int[]? categoryId { get; set; }
        public string? search { get; set; }
        public int[]? Ratings { get; set; }
        public bool? inStock { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public string? sort { get; set; }
        public string sortOrder { get; set; } = "asc";
    }


      public class CategoryResDto
    {
         public int Id { get; set; }
         
         public string Name {  set; get; }

        public ImageDtoRes? Image {  get; set; }
                 
    }

    public class BrandResDto
    {
        public int Id { get; set; }

        public string Name { set; get; }

        public ImageDtoRes? Image { get; set; }

    }

    public class ProductPagination:Pagination<Product>
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

    }

    public class ProductPaginationRes : Pagination<ProductResDto>
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

    }

}
