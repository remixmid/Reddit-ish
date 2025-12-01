using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    
    private readonly string _filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
    public async Task<Post> AddAsync(Post post)
    {
        string postAsJson = await File.ReadAllTextAsync(_filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 1;
        post.Id = maxId + 1;
        posts.Add(post);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(_filePath, postAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postAsJson = await File.ReadAllTextAsync(_filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        Post? existingPost = posts.SingleOrDefault(x => x.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"The Post with id {post.Id} was not found.");
        }
        posts.Remove(existingPost);
        posts.Add(post);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(_filePath, postAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string postAsJson = await File.ReadAllTextAsync(_filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson);
        Post? existingPost = posts.SingleOrDefault(x => x.Id == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"The Post with id {id} was not found.");
        }

        posts.Remove(existingPost);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(_filePath, postAsJson);
    }

    public Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        foreach (var p in posts)
        {
            if (p.Id == id)
                return Task.FromResult(p);
        }
        throw new InvalidOperationException($"The Post with id {id} was not found.");
    }

    public IQueryable<Post> GetManyAsync()
    {
        string postsAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        return posts.AsQueryable();
    }
}