using System.Text.Json.Serialization;

namespace server.Entities
{
    public class WishListItems
    {
        public int Id { get; set; }
        public int ProductId{ get; set; }
        public Product Product{ get; set; }
        public int WishListId { get; set; }
        [JsonIgnore]
        public WishList WishList { get; set; }

    }
}
