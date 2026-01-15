using server.Entities;

public class WishList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public ICollection<WishListItems> WishListItems { get; set; } = new List<WishListItems>();
}
