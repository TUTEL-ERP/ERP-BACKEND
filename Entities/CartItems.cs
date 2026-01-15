using System.Text.Json.Serialization;

namespace server.Entities
{
    public class CartItems
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CartId { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; }
    }
}
