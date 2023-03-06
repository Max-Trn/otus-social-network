using social_network.DAL.Models;
using social_network.Models.Requests;

namespace social_network.Services.Interfaces;

public interface IUserService
{
    Task<UserModel> Get(int id);
    Task<int> Register(UserRegisterRequest model);
}