using dialogs_api.DAL.Models;

namespace social_network.HttpClients.Interfaces;

public interface IDialogClient
{
    Task SendMessage(long userId, long friendId, string message);
    
    Task<List<DialogMessage>> GetDialog(long userId, long friendId);
}