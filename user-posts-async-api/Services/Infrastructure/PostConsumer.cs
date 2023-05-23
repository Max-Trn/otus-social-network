using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace user_posts_async_api.Services.Infrastructure;

public class PostConsumer : BackgroundService  
{  
    private readonly IModel _channel;
    private readonly PostWebSocketService _socketService;
    private IConnection _connection;  
    
    public PostConsumer(PostWebSocketService socketService)  
    {  
        _socketService = socketService;
        Console.WriteLine("Consuming Queue Now");

        ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
        factory.UserName = "guest";
        factory.Password = "guest";
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare("ex", ExchangeType.Direct);
        _channel.QueueDeclare(queue: "feed-post",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }  
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)  
    {  
        stoppingToken.ThrowIfCancellationRequested();  
  
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received from Rabbit: {0}", message);
            
            await _socketService.Send(ea.RoutingKey, body);

        };
        _channel.BasicConsume(queue: "feed-post",
            autoAck: true,
            consumer: consumer);
        
        return Task.CompletedTask;  
    }

    public override void Dispose()  
    {  
        _channel.Close();  
        _connection.Close();  
        base.Dispose();  
    } 

    public void BindClient(string userId)
    {
        _channel.QueueBind(queue: "feed-post", exchange: "ex", routingKey: $"{userId}");
    }
}  
