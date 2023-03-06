namespace social_network.DAL.Models;

public class UserModel
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string SecondName { get; set; }

    public int Age { get; set; }

    public string Biography { get; set; }

    public string City { get; set; }

    public string Password { get; set; }
}