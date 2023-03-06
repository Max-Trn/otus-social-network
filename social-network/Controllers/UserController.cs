using Microsoft.AspNetCore.Mvc;
using social_network.Models.Requests;
using social_network.Services.Interfaces;

namespace social_network.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> Get(int userId)
    {
        try
        {
            var user = await _userService.Get(userId);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest model)
    {
        try
        {
            var id = await _userService.Register(model);
            return Ok(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
}