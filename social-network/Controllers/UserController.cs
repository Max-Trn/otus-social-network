using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using social_network.Exceptions;
using social_network.Models.Requests;
using social_network.Services.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace social_network.Controllers;

[Authorize]
[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [SwaggerOperation(Summary = "Получение анкеты пользователя")]
    [HttpGet("get/{userId}")]
    public async Task<IActionResult> Get(int userId)
    {
        var user = await _userService.Get(userId);
        if (user is null)
        {
            return NotFound("Пользователь не найден");
        }

        return Ok(user);
    }

    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns>ID нового пользователя</returns>
    [SwaggerOperation(Summary = "Регистрация нового пользователя")]
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserRegisterRequest model)
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

    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Token</returns>
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Упрощенный процесс аутентификации путем передачи идентификатор пользователя и получения токена для дальнейшего прохождения авторизации",
        Description = "Возвращает Token")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserAuthenticateRequest model)
    {
        try
        {
            var user = await _userService.Authenticate(model.UserId, model.Password);
            var token = _userService.GetUserToken(user);

            return Ok(token);
        }
        catch (UserNotFoundException e)
        {
            return NotFound("Пользователь не найден");
        }
        catch (Exception e)
        {
            return BadRequest("Невалидные данные");
        }
    }
    
    /// <summary>
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Token</returns>
    [SwaggerOperation(Summary = "Запрос поиска пользователей по части имени и фамилии",
        Description = "Возвращает список пользователей")]
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] UsersSearchRequest model)
    {
        try
        { 
            var users = await _userService.Search(model);

            return Ok(users);
        }
        catch (InputDataIncorrect e)
        {
            return BadRequest("Невалидные данные");
        }
    }
}