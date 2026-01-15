
namespace server.Entities
{
    public class Product:AuditBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OrignalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }

        public decimal NewPrice
        {
            get
            {
                if (DiscountPercentage.HasValue && DiscountPercentage.Value > 0)
                {
                    return OrignalPrice - (OrignalPrice * DiscountPercentage.Value / 100);
                }

                if (DiscountAmount.HasValue && DiscountAmount.Value > 0)
                {
                    return OrignalPrice - DiscountAmount.Value;
                }

                return OrignalPrice;
            }
        }


        public bool IsOnDiscount  { 
            
            get 
            { 
               return DiscountPercentage.HasValue  && DiscountPercentage.Value > 0 
                    || DiscountAmount.HasValue && DiscountAmount.Value > 0;


            } }



        public int StockQuantity { get; set; }
        public double AvearageRating { get; set; }
        public int TotalReviews { get; set; }
        public bool InStock { get { return StockQuantity > 0 ? true : false; } }
        public bool IsFeatured { get; set; } = false;


        public int CategoryId { get; set; }
        public Categories Categories { get; set; }  
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
            
            
        public ICollection<ProductReview> ProductReviews { get; set; }

        public int? ThumbnailId { get; set; }
        public Image? Thumbnail { get; set; }

       
    }
}   
