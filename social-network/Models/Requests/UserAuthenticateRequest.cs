namespace social_network.Models.Requests;

public class UserAuthenticateRequest
{
    public int UserId { get; set; }
    
    public string Password { get; set; }
}