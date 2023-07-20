using social_network.DAL.Models;
using social_network.DAL.Repositories;
using social_network.Services.Interfaces;

namespace social_network.Services;

public class DialogService : IDialogService
{
    private readonly DialogRepository _dialogRepository;

    public DialogService(DialogRepository dialogRepository)
    {
        _dialogRepository = dialogRepository;
    }
    
    public async Task Send(int userIdFrom, int userIdTo, string text)
    {
        await _dialogRepository.Add(userIdFrom, userIdTo, text);
    }
    
    public async Task<List<DialogMessage>> List(int userIdFrom, int userIdTo)
    {
        return await _dialogRepository.GetMessages(userIdFrom, userIdTo);
    }
}