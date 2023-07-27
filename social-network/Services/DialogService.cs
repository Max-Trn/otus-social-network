using dialogs_api.DAL.Models;
using social_network.HttpClients.Interfaces;
using social_network.Services.Interfaces;

namespace social_network.Services;

public class DialogService : IDialogService
{
    private readonly IDialogClient _dialogClient;
    
    public DialogService(IDialogClient dialogClient)
    {
        _dialogClient = dialogClient;
    }
    
    public async Task Send(int userIdFrom, int userIdTo, string text)
    {
        await _dialogClient.SendMessage(userIdFrom, userIdTo, text);
    }
    
    public async Task<List<DialogMessage>> List(int userIdFrom, int userIdTo)
    {
        return await _dialogClient.GetDialog(userIdFrom, userIdTo);
    }
}