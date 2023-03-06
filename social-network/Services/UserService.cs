using social_network.DAL.Models;
using social_network.DAL.Repositories;
using social_network.Models.Requests;
using social_network.Services.Interfaces;

namespace social_network.Services;

public class UserService: IUserService
{
    private readonly UserRepository _userRepository;
    
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserModel> Get(int id)
    {
        return await _userRepository.Get(id);
    }

    public async Task<int> Register(UserRegisterRequest model)
    {
        //TODO: Сделать шифрование пароля
        return await _userRepository.Add(new UserModel()
        {
            Age = model.Age,
            Biography = model.Biography,
            City = model.City,
            Password = model.Password,
            FirstName = model.FirstName,
            SecondName = model.SecondName
        });
    }
}