using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using social_network.Configuration;
using social_network.DAL.Models;
using social_network.DAL.Repositories;
using social_network.Exceptions;
using social_network.Models.Requests;
using social_network.Services.Interfaces;

namespace social_network.Services;

public class UserService: IUserService
{
    private readonly UserRepository _userRepository;
    private readonly ApplicationConfiguration _configuration;
    
    public UserService(UserRepository userRepository, ApplicationConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    
    public async Task<UserModel> Get(int id)
    {
        return await _userRepository.Get(id);
    }

    public async Task<UserModel[]> Search(UsersSearchRequest model)
    {
        return await _userRepository.Search(model);
    }

    public async Task<int> Register(UserRegisterRequest model)
    {
        var hashedPassword = new PasswordHasher<object?>().HashPassword(null, model.Password);
        return await _userRepository.Add(new UserModel()
        {
            Age = model.Age,
            Biography = model.Biography,
            City = model.City,
            Password = hashedPassword,
            FirstName = model.FirstName,
            SecondName = model.SecondName
        });
    }
    
    public async Task<UserModel> Authenticate(int userId, string password)
    {
        var user = await _userRepository.Get(userId);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        if (!IsPasswordCorrect(user.Password, password))
        {
            throw new InputDataIncorrect();;
        }

        return user;
    }

    public string GetUserToken(UserModel user)
    {
        var issuer = "test";
        var audience ="test";
        var key = Encoding.ASCII.GetBytes(_configuration.ApplicationKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }

    private bool IsPasswordCorrect(string hashedPassword, string password)
    {
        var passwordVerificationResult = new PasswordHasher<object?>().VerifyHashedPassword(null, hashedPassword, password);
        switch (passwordVerificationResult)
        {
            case PasswordVerificationResult.Failed:
                return false;
            
            case PasswordVerificationResult.SuccessRehashNeeded:
            case PasswordVerificationResult.Success:
                return true;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}