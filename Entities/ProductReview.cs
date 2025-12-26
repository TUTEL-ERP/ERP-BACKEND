namespace server.Entities
{
    public class ProductReview : AuditBase

    {

        public int Id { get; set; }
        public string Review { get; set; }
        public int ProductId { get; set; }
        public Product Product{ get; set; }


    }

}
