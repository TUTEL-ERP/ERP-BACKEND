using server.Entities;

namespace server.Dto
{
    public class AddToCartReqDto
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }
    }


    public class AddToCartResDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductResDto Product { get; set; }
    }
}
