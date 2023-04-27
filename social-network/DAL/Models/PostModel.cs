namespace social_network.DAL.Models;

public class PostModel
{
    public long Id { get; set; }

    public long UserId { get; set; }
    
    public string Text { get; set; }
}