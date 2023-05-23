namespace social_network.Services.Interfaces;

public interface IMessageService
{
    bool Enqueue(string messageString, long userId);
}