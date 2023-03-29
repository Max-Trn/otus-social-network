using social_network.DAL.Models;
using social_network.Models.Requests;

namespace social_network.Services.Interfaces;

public interface IUserService
{
    Task<UserModel> Get(int id);
    Task<UserModel[]> Search(UsersSearchRequest model);
    Task<int> Register(UserRegisterRequest model);
    Task<UserModel> Authenticate(int userId, string password);
    string GetUserToken(UserModel user);
}