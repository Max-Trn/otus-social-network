using social_network.DAL.Models;

namespace social_network.Services.Interfaces;

public interface IDialogService
{
    Task Send(int userIdFrom, int userIdTo, string text);
    Task<List<DialogMessage>> List(int userIdFrom, int userIdTo);
}