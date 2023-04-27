using System.Data;
using Dapper;
using Npgsql;
using social_network.DAL.Infrastructure;
using social_network.DAL.Models;
using social_network.Models.Requests;

namespace social_network.DAL.Repositories;

public class UserRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public UserRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public async Task<UserModel> Get(int id)
    {
        string commandText = $"SELECT * FROM users WHERE ID = @id";

        var queryArgs = new { Id = id };
        var user = await _connectionFactory.MasterConnection.QueryFirstOrDefaultAsync<UserModel>(commandText, queryArgs);
        return user;
    }
    
    public async Task<int> Add(UserModel userModel)
    {
        var commandText = "insert into users (biography, password, city, first_name, second_name, age)" +
                                "values (@biography, @password, @city, @firstName, @secondName, @age) returning id";

        return await _connectionFactory.MasterConnection.QuerySingleAsync<int>(commandText, userModel);
    }

    public async Task<UserModel[]> Search(UsersSearchRequest model)
    {
        string commandText = $"SELECT * FROM users WHERE first_name like @firstName||'%' and second_name like @lastName||'%' order by id";
        var users = await _connectionFactory.ReplicaConnection.QueryAsync<UserModel>(commandText, model);
        return users.ToArray();
    }
}