using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_network.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace social_network.Controllers;

[Authorize]
[Route("post")]
public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly IUserService _userService;
    
    public PostController(IPostService postService, IUserService userService)
    {
        _postService = postService;
        _userService = userService;
    }
    
    /// <summary>
    /// </summary>
    /// <param name="Текст поста"></param>
    [SwaggerOperation(Summary = "Добавить новый пост")]
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> Сreate([FromBody]string text)
    {
        try
        {
            var currentUserId = _userService.GetCurrentUserId();
            var id = await _postService.Create(currentUserId,text);
            return Ok(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// </summary>
    /// <param name="Текст поста"></param>
    [SwaggerOperation(Summary = "Обновления друзей")]
    [AllowAnonymous]
    [HttpPost("feed")]
    public async Task<IActionResult> GetFeed()
    {
        try
        {
            var currentUserId = _userService.GetCurrentUserId();
            var id = await _postService.GetFeedFromCache(currentUserId);
            return Ok(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}