using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using social_network.DAL.Models;
using social_network.DAL.Repositories;
using social_network.Services.Interfaces;

namespace social_network.Services;

public class PostService : IPostService
{
    private readonly PostRepository _postRepository;
    private readonly FriendRepository _friendRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly IMessageService _messageService;
    
    public PostService(PostRepository postRepository,
        IMemoryCache memoryCache,
        FriendRepository friendRepository,
        IMessageService messageService)
    {
        _postRepository = postRepository;
        _memoryCache = memoryCache;
        _friendRepository = friendRepository;
        _messageService = messageService;
    }
    
    
    public async Task<long> Create(long userId, string text)
    {
        var model = new PostModel()
        {
            UserId = userId,
            Text = text
        };
        
        var friends = await _friendRepository.GetFriendsByUserId(userId);
        foreach (var friendId in friends)
        {
            _messageService.Enqueue(JsonSerializer.Serialize(model), friendId);
        }
        
        return await _postRepository.Add(model);
    }

    public async Task Delete(long postId)
    {
       await _postRepository.Delete(postId);
    }

    public async Task<PostModel> Get(long postId)
    {
       return await _postRepository.Get(postId);
    }

    public async Task<List<PostModel>> GetFeedFromCache(int userId)
    {
        var userFeedKey = GetUserFeedKey(userId);
        if (!_memoryCache.TryGetValue(userFeedKey, out List<PostModel> feed))
        {
            feed = await MakeFeed(userId);
            _memoryCache.Set(userFeedKey, feed, TimeSpan.FromMinutes(5));
        }

        return feed;
    }

    private async Task<List<PostModel>> MakeFeed(long userId)
    {
        var friendsIds = await _friendRepository.GetFriendsByUserId(userId);

        var feed = new List<PostModel>();
        foreach (var friendId in friendsIds)
        {
            var friendPosts = await GetUserPostsFromCache(friendId);
            feed.AddRange(friendPosts);
        }

        feed = feed.OrderBy(x => x.Id).ToList();

        return feed.Count > 1000? feed.GetRange(0, 1000): feed;
    }

    private async Task<List<PostModel>> GetUserPostsFromCache(long userId)
    {
        var userPostsKey = GetUserPostsKey(userId);
        if (!_memoryCache.TryGetValue(userPostsKey, out List<PostModel> userPosts))
        {
            userPosts = await _postRepository.GetByUserId(userId);
            _memoryCache.Set(userPostsKey, userPosts, TimeSpan.FromMinutes(5));
        }

        return userPosts;
    }

    private string GetUserPostsKey(long userId) => "user" + userId;
    
    private string GetUserFeedKey(long userId) => "userFeed" + userId;
}