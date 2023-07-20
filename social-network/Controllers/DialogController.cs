using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_network.Services;
using social_network.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace social_network.Controllers;

[Authorize]
[Route("dialog")]
public class DialogController : Controller
{
    private readonly IDialogService _dialogService;
    private readonly IUserService _userService;
    
    public DialogController(IDialogService dialogService, IUserService userService)
    {
        _dialogService = dialogService;
        _userService = userService;
    }
    
    /// <summary>
    /// </summary>
    /// <param name="friendId">айди друга</param>
    /// <param name="text">текст сообщения</param>
    [SwaggerOperation(Summary = "отправить сообщение")]
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromQuery] int friendId, [FromBody] string text)
    {
        try
        {
            var currentUserId = _userService.GetCurrentUserId();
            await _dialogService.Send(currentUserId, friendId, text);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// </summary>
    /// <param name="friendId">айди друга</param>
    /// <param name="text">текст сообщения</param>
    [SwaggerOperation(Summary = "получить диалог пользователей")]
    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] int friendId)
    {
        try
        {
            var currentUserId = _userService.GetCurrentUserId();
            var messages = await _dialogService.List(currentUserId, friendId);
            return Ok(messages);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}