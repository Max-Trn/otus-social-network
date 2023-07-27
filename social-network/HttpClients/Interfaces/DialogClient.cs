using dialogs_api.DAL.Models;

namespace social_network.HttpClients.Interfaces;

public class DialogClient : HttpClient, IDialogClient
{
    public async Task SendMessage(long userId, long friendId, string message)
    {
        var data = new { UserId = userId, FriendId = friendId, Text = message };
        JsonContent content = JsonContent.Create(data);
        
        await PostAsync("http://localhost:7289/", content);
    }

    public async Task<List<DialogMessage>> GetDialog(long userId, long friendId)
    {
        var data = new { UserId = userId, FriendId = friendId };
        JsonContent content = JsonContent.Create(data);
        
        var response =  await PostAsync("http://localhost:7289/", content);
        
        List<DialogMessage> result = await response.Content.ReadFromJsonAsync<List<DialogMessage>>();

        return result;
    }
}