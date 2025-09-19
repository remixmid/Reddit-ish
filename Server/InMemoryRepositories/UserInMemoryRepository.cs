using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    List<User> _users = new List<User>();
    
    public Task<User> CreateAsync(User user)
    {
        int maxId = 1;
        foreach (var item in _users)
        {
            if (item.Id > maxId)
            {
                maxId = item.Id;
            }
        }
        user.Id = maxId + 1;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        foreach (var item in _users)
        {
            if (item.Id == user.Id)
            {
                _users.Remove(item);
                _users.Add(user);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"User with id {user.Id} was not found"
        );
    }

    public Task DeleteAsync(User user)
    {
        foreach (var item in _users)
        {
            if (item.Id == user.Id)
            {
                _users.Remove(item);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"User with id {user.Id} was not found"
        );
    }

    public Task<User> GetSingleAsync(int id)
    {
        foreach (var item in _users)
        {
            if (item.Id == id)
            {
                return Task.FromResult(item);
            }
        }

        throw new InvalidOperationException(
            $"User with id {id} was not found"
        );
    }
    
    public Task<User> GetSingleAsync(string username)
    {
        foreach (var item in _users)
        {
            if (item.Username == username)
            {
                return Task.FromResult(item);
            }
        }

        throw new InvalidOperationException(
            $"User with id {username} was not found"
        );
    }

    public IQueryable<User> GetManyAsync()
    {
        return _users.AsQueryable();
    }
}