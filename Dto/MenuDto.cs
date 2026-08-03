// Dto/MenuDto.cs
namespace server.Dto
{
    public class MenuDto
    {
        public int MenuId { get; set; }
        public int? ParentMenuId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public string? Route { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<MenuDto> Children { get; set; } = new();
        public bool Expanded { get; set; } = false;
    }
}