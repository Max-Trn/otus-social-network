using System.Data;
using Dapper;
using Npgsql;
using social_network.DAL.Models;
using social_network.Models.Requests;

namespace social_network.DAL.Repositories;

public class UserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        _dbConnection = dbConnection;
    }
    
    public async Task<UserModel> Get(int id)
    {
        string commandText = $"SELECT * FROM users WHERE ID = @id";

        var queryArgs = new { Id = id };
        var user = await _dbConnection.QueryFirstOrDefaultAsync<UserModel>(commandText, queryArgs);
        return user;
    }
    
    public async Task<int> Add(UserModel userModel)
    {
        var commandText = "insert into users (biography, password, city, first_name, second_name, age)" +
                                "values (@biography, @password, @city, @firstName, @secondName, @age) returning id";

        return await _dbConnection.QuerySingleAsync<int>(commandText, userModel);
    }

    public async Task<UserModel[]> Search(UsersSearchRequest model)
    {
        string commandText = $"SELECT * FROM users WHERE first_name like @firstName||'%' and second_name like @lastName||'%' order by id";
        var users = await _dbConnection.QueryAsync<UserModel>(commandText, model);
        return users.ToArray();
    }
}