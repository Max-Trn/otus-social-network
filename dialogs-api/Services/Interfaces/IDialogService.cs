using dialogs_api.DAL.Models;

namespace dialogs_api.Services.Interfaces;

public interface IDialogService
{
    Task Send(int userIdFrom, int userIdTo, string text);
    Task<List<DialogMessage>> List(int userIdFrom, int userIdTo);
}