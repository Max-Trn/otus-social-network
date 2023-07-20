using Dapper;
using social_network.DAL.Infrastructure;
using social_network.DAL.Models;

namespace social_network.DAL.Repositories;

public class DialogRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public DialogRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public async Task Add(long userId, long friendId, string text)
    {
        var commandText = "insert into dialog (user_id, friend_id, text, shard_key)" +
                          "values (@userId, @friendId, @text, @shardKey)";

        var queryArgs = new
        {
            userId = userId,
            friendId = friendId,
            text = text,
            shardKey = userId + friendId
        };
        await _connectionFactory.MasterConnection.ExecuteAsync(commandText, queryArgs);
    }
    
    public async Task<List<DialogMessage>> GetMessages(long userId, long friendId)
    {
        var commandText =
            "select * from dialog where user_id = @userId and friend_id = @friendId or user_id = @friendId and friend_id = @userId";

        var queryArgs = new
        {
            userId = userId,
            friendId = friendId,
        };
        var result = await _connectionFactory.MasterConnection.QueryAsync<DialogMessage>(commandText, queryArgs);

        return result.ToList();
    }
}