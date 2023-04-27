using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_network.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace social_network.Controllers;

[Authorize]
[Route("friend")]
public class FriendController : Controller
{
    private readonly IUserService _userService;

    public FriendController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// </summary>
    /// <param name="айди друга"></param>
    [SwaggerOperation(Summary = "добавить друга")]
    [AllowAnonymous]
    [HttpPost("add")]
    public async Task<IActionResult> AddFriend(int friendId)
    {
        try
        {
            await _userService.AddFriend(friendId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// </summary>
    /// <param name="айди друга"></param>
    [SwaggerOperation(Summary = "удалить друга")]
    [AllowAnonymous]
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteFriend(int friendId)
    {
        try
        {
            await _userService.DeleteFriend(friendId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}