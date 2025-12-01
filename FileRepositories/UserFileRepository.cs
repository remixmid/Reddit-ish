using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    
    private readonly string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
    public async Task<User> AddAsync(User user)
    {
        string userAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 1;
        user.Id = maxId + 1;
        users.Add(user);
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, userAsJson);
        return user;
    }
    
    public async Task UpdateAsync(User user)
    {
        string userAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        User? existingUser = users.SingleOrDefault(x => x.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"The User with id {user.Id} was not found.");
        }
        users.Remove(existingUser);
        users.Add(user);
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, userAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string userAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        User? existingUser = users.SingleOrDefault(x => x.Id == id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"The User with id {id} was not found.");
        }

        users.Remove(existingUser);
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, userAsJson);
    }

    public Task<User> GetSingleAsync(int id)
    {
        string userAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        foreach (var u in users)
        {
            if (u.Id == id)
                return Task.FromResult(u);
        }
        throw new InvalidOperationException($"The User with id {id} was not found.");
    }

    public IQueryable<User> GetManyAsync()
    {
        string userAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson);
        return users.AsQueryable();
    }
}