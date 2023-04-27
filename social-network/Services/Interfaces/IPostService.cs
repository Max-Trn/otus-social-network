using social_network.DAL.Models;

namespace social_network.Services.Interfaces;

public interface IPostService
{
    Task<long> Create(long userId, string text);
    
    Task Delete(long postId);
    
    Task<PostModel> Get(long postId);

    Task<List<PostModel>> GetFeedFromCache(int userId);
}