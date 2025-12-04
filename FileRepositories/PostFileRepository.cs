using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string _filePath = "posts.json";
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public PostFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            // пустой список постов
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<Post>> ReadAllAsync()
    {
        string json = await File.ReadAllTextAsync(_filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(json, _jsonOptions);
        return posts ?? new List<Post>();
    }

    private async Task WriteAllAsync(List<Post> posts)
    {
        string json = JsonSerializer.Serialize(posts, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<Post> AddAsync(Post post)
    {
        var posts = await ReadAllAsync();

        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 0;
        post.Id = maxId + 1;

        posts.Add(post);
        await WriteAllAsync(posts);

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var posts = await ReadAllAsync();

        var existing = posts.SingleOrDefault(x => x.Id == post.Id);
        if (existing is null)
            throw new InvalidOperationException($"The Post with id {post.Id} was not found.");

        posts.Remove(existing);
        posts.Add(post);

        await WriteAllAsync(posts);
    }

    public async Task DeleteAsync(int id)
    {
        var posts = await ReadAllAsync();

        var existing = posts.SingleOrDefault(x => x.Id == id);
        if (existing is null)
            throw new InvalidOperationException($"The Post with id {id} was not found.");

        posts.Remove(existing);
        await WriteAllAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var posts = await ReadAllAsync();

        var existing = posts.SingleOrDefault(x => x.Id == id);
        if (existing is null)
            throw new InvalidOperationException($"The Post with id {id} was not found.");

        return existing;
    }

    public IQueryable<Post> GetManyAsync()
    {
        // здесь можешь сделать async-версию, но раз интерфейс возвращает IQueryable —
        // читаем синхронно (быстро) и возвращаем список
        string json = File.ReadAllText(_filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(json, _jsonOptions) ?? new List<Post>();
        return posts.AsQueryable();
    }
}
