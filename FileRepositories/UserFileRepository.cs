using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string _filePath = "users.json";
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<User>> ReadAllAsync()
    {
        string json = await File.ReadAllTextAsync(_filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions);
        return users ?? new List<User>();
    }

    private async Task WriteAllAsync(List<User> users)
    {
        string json = JsonSerializer.Serialize(users, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public async Task<User> AddAsync(User user)
    {
        var users = await ReadAllAsync();

        int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
        user.Id = maxId + 1;

        users.Add(user);
        await WriteAllAsync(users);

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var users = await ReadAllAsync();

        var existing = users.SingleOrDefault(u => u.Id == user.Id);
        if (existing is null)
            throw new InvalidOperationException($"User with id {user.Id} was not found.");

        users.Remove(existing);
        users.Add(user);

        await WriteAllAsync(users);
    }

    public async Task DeleteAsync(int id)
    {
        var users = await ReadAllAsync();

        var existing = users.SingleOrDefault(u => u.Id == id);
        if (existing is null)
            throw new InvalidOperationException($"User with id {id} was not found.");

        users.Remove(existing);
        await WriteAllAsync(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var users = await ReadAllAsync();

        var existing = users.SingleOrDefault(u => u.Id == id);
        if (existing is null)
            throw new InvalidOperationException($"User with id {id} was not found.");

        return existing;
    }

    public IQueryable<User> GetManyAsync()
    {
        string json = File.ReadAllText(_filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions) ?? new List<User>();
        return users.AsQueryable();
    }
    
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        var users = await ReadAllAsync();

        var user = users.SingleOrDefault(u =>
            u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

        return user;
    }

}
