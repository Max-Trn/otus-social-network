using Dapper;
using social_network.DAL.Infrastructure;

namespace social_network.DAL.Repositories;

public class FriendRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public FriendRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public async Task<int> Add(long userId, long friendId)
    {
        var commandText = "insert into friend (user_id, friend_id)" +
                          "values (@userId, @friendId) returning id";

        var queryArgs = new
        {
            userId = userId,
            friendId = friendId
        };
        return await _connectionFactory.MasterConnection.QuerySingleAsync<int>(commandText, queryArgs);
    }
    
    public async Task Delete(long userId, long friendId)
    {
        var commandText = "delete from friend where user_id = @userId and friend_id = @friendId";

        var queryArgs = new
        {
            userId = userId,
            friendId = friendId
        };
        await _connectionFactory.MasterConnection.ExecuteAsync(commandText, queryArgs);
    }
    
    public async Task<List<long>> GetFriendsByUserId(long userId)
    {
        var commandText = "select friend_id from friend where user_id = @userId";

        var queryArgs = new
        {
            userId = userId
        };
        return (await _connectionFactory.MasterConnection.QueryAsync<long>(commandText, queryArgs)).ToList();
    }
}