using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Swashbuckle.AspNetCore.Annotations;
using user_posts_async_api.Services;
using user_posts_async_api.Services.Infrastructure;
using user_posts_async_api.Services.Interfaces;

namespace user_posts_async_api.Controllers;

[Authorize]
[Route("post")]
public class PostController : Controller
{
    private readonly IUserService _userService;
    private readonly PostWebSocketService _socketService;
    private readonly PostConsumer _postConsumer;

    public PostController(
        PostConsumer postConsumer,
        IUserService userService,
        PostWebSocketService socketService)
    {
        _postConsumer = postConsumer;
        _userService = userService;
        _socketService = socketService;
    }
    
    /// <summary>
    /// </summary>
    /// <param name="Канал используется для быстрого обновления ленты постов от друзей пользователя"></param>
    [SwaggerOperation(Summary = "Событие публикации поста одного из друзей пользователя")]
    [AllowAnonymous]
    [HttpGet("feed/posted")]
    public async Task<IActionResult> Posted()
    {
        var userId = _userService.GetCurrentUserId();
        _postConsumer.BindClient(userId.ToString());
        
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
           using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
           
           _socketService.AddClient(userId.ToString(), webSocket);
           while (true)
           {
               
           }
        }
        
        return Ok();
    }
}