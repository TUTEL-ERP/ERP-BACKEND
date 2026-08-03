// Models/Entities/Menu.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Enity
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }

        public int? ParentMenuId { get; set; }

        [Required]
        [MaxLength(100)]
        public string MenuName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Icon { get; set; }

        [MaxLength(200)]
        public string? Route { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(ParentMenuId))]
        public virtual Menu? ParentMenu { get; set; }

        public virtual ICollection<Menu> Children { get; set; } = new List<Menu>();
    }
}