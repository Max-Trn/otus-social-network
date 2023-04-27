namespace social_network.Models.Requests;

public class GetFeedRequest
{
    public int Offset { get; set; }
    
    public int Limit { get; set; }
}