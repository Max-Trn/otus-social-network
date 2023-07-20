namespace social_network.DAL.Models;

public class DialogMessage
{
    public int UserId { get; set; }
    
    public int FriendId { get; set; }
    
    public string Text { get; set; }
}