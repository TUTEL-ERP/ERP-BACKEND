using System.Text.Json;

namespace server.Dto
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplierCode { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? NTN { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class SupplierRequestDto
    {
        public string? formId { get; set; }
        public JsonElement data { get; set; }
        public string? recordId { get; set; }
    }
}