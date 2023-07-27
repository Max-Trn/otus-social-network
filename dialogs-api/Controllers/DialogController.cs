using dialogs_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace dialogs_api.Controllers;

[Route("dialog")]
public class DialogController : Controller
{
    private readonly IDialogService _dialogService;
    private readonly ILogger _logger;
    
    public DialogController(
        IDialogService dialogService,
        ILogger logger)
    {
        _dialogService = dialogService;
        _logger = logger;
    }
    
    /// <summary>
    /// </summary>
    /// <param name="friendId">айди друга</param>
    /// <param name="text">текст сообщения</param>
    [SwaggerOperation(Summary = "отправить сообщение")]
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromQuery] int friendId, [FromQuery] int userId, [FromBody] string text)
    {
        try
        {
            await _dialogService.Send(userId, friendId, text);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError("Dialog API: ${userIdFrom} send message to ${userIdTo} failed");
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// </summary>
    /// <param name="friendId">айди друга</param>
    /// <param name="text">текст сообщения</param>
    [SwaggerOperation(Summary = "получить диалог пользователей")]
    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] int friendId, [FromQuery] int userId)
    {
        try
        {
            _logger.LogError("Dialog API: ${userIdFrom} get messages to ${userIdTo} failed");
            
            var messages = await _dialogService.List(userId, friendId);
            return Ok(messages);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}