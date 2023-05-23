using System.Text;
using RabbitMQ.Client;
using social_network.Services.Interfaces;

namespace social_network.Services;

public class MessageService : IMessageService
{
    ConnectionFactory _factory;
    IConnection _conn;
    IModel _channel;
    public MessageService()
    {
        Console.WriteLine("about to connect to rabbit");

        _factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
        _factory.UserName = "guest";
        _factory.Password = "guest";
        _conn = _factory.CreateConnection();
        _channel = _conn.CreateModel();
        _channel.QueueDeclare(queue: "feed-post",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        var ttl = new Dictionary<string, object>
        {
            { "x-message-ttl", 30000 }
        };
        _channel.ExchangeDeclare("ex", ExchangeType.Direct, arguments: ttl);
    }
    
    public bool Enqueue(string messageString, long userId)
    {
        var body = Encoding.UTF8.GetBytes(messageString);
        _channel.BasicPublish(exchange: "ex",
            routingKey: userId.ToString(),
            basicProperties: null,
            body: body);
        Console.WriteLine(" [x] Published {0} to RabbitMQ", messageString);
        return true;
    }
}