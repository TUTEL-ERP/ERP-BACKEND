using server.Entities;

namespace server.Dto
{
    public class AddWishListitemDto
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }
    }


    public class WishListitemResDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductResDto Product { get; set; }   
    }
}
