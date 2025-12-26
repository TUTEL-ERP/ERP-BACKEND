namespace server.Entities
{
    public abstract class AuditBase
    {
        public DateTime? Created { get; set; } = DateTime.UtcNow;
        public int CreatedBy { get; set; }
        public int CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
         public int ModifiedBy { get; set; }
         public bool isDelete { get; set; }
     
    }
}

