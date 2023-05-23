using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace user_posts_async_api.Services;

public class PostWebSocketService
{
    private ConcurrentDictionary<string, WebSocket> _clients = new ConcurrentDictionary<string, WebSocket>();

    public async Task Send(string userId, byte[] message)
    {
        if (!_clients.ContainsKey(userId))
        {
            return;
        }

        var webSocket = _clients[userId];
        await webSocket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
        
        Console.WriteLine(" [x] Sent to WebSocket: {0}");
    }

    public void AddClient(string userId, WebSocket socket)
    {
        _clients.TryAdd(userId, socket);
    }
}