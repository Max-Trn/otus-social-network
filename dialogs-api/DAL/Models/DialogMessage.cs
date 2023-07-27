namespace dialogs_api.DAL.Models;

public class DialogMessage
{
    public int UserId { get; set; }
    
    public int FriendId { get; set; }
    
    public string Text { get; set; }
}