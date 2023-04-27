using Dapper;
using social_network.DAL.Infrastructure;
using social_network.DAL.Models;

namespace social_network.DAL.Repositories;

public class PostRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public PostRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public async Task<int> Add(PostModel postModel)
    {
        var commandText = "insert into posts (user_id, text)" +
                          "values (@userId, @text) returning id";

        return await _connectionFactory.MasterConnection.QuerySingleAsync<int>(commandText, postModel);
    }
    
    public async Task Delete(long postId)
    {
        var commandText = "delete from posts where id = @postId";

        var queryArgs = new { PostId = postId };
        await _connectionFactory.MasterConnection.ExecuteAsync(commandText, queryArgs);
    }
    
    public async Task<PostModel> Get(long id)
    {
        string commandText = $"SELECT * FROM posts WHERE ID = @id";

        var queryArgs = new { Id = id };
        var post = await _connectionFactory.MasterConnection.QueryFirstOrDefaultAsync<PostModel>(commandText, queryArgs);
        return post;
    }
    
    public async Task<List<PostModel>> GetByUserId(long userId)
    {
        string commandText = $"SELECT * FROM posts WHERE user_id = @UserId";

        var queryArgs = new { UserId = userId };
        var posts = (await _connectionFactory.MasterConnection.QueryAsync<PostModel>(commandText, queryArgs)).ToList();
        return posts;
    }
}