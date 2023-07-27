using dialogs_api.DAL.Models;
using dialogs_api.DAL.Repositories;
using dialogs_api.Services.Interfaces;

namespace dialogs_api.Services;

public class DialogService : IDialogService
{
    private readonly DialogRepository _dialogRepository;
    private readonly ILogger _logger;
    
    public DialogService(
        DialogRepository dialogRepository,
        ILogger logger)
    {
        _dialogRepository = dialogRepository;
        _logger = logger;
    }
    
    public async Task Send(int userIdFrom, int userIdTo, string text)
    {
        await _dialogRepository.Add(userIdFrom, userIdTo, text);
        _logger.LogInformation("Dialog API: ${userIdFrom} sent message to ${userIdTo}");
    }
    
    public async Task<List<DialogMessage>> List(int userIdFrom, int userIdTo)
    {
        _logger.LogInformation("Dialog API: get messages from ${userIdFrom} - ${userIdTo} ");
        return await _dialogRepository.GetMessages(userIdFrom, userIdTo);
    }
}