using System.Text.Json.Serialization;

namespace server.Entities
{
    public class Image:AuditBase
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageName { get; set; }
        public string? ImageExtension { get; set; }


        [JsonIgnore]
        public Categories Category { get; set; }
        [JsonIgnore]
        public Brand Brand { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

    }
}
