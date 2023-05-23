using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using user_posts_async_api.Services.Interfaces;

namespace user_posts_async_api.Services;

public class UserService: IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public UserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public int GetCurrentUserId()
    { 
        var id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(id);
    }
}