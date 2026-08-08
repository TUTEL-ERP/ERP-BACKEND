using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Enity
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CountryName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}