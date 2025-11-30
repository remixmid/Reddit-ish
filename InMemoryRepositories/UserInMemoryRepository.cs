using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    public UserInMemoryRepository()
    {
        users.Add(new User(1, "FirstTestUser", "FirstTestPassword"));
        users.Add(new User(2, "SecondTestUser", "SecondTestPassword"));
        users.Add(new User(3, "ThirdTestUser", "ThirdTestPassword"));
        users.Add(new User(4, "FourthTestUser", "FourthTestPassword"));
        users.Add(new User(5, "FifthTestUser", "FifthTestPassword"));
    }

    public List<User> users { get; } = new();


    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(x => x.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with id {user.Id} does not exist");
        }

        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? existingUser = users.SingleOrDefault(x => x.Id == id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with id {id} does not exist");
        }

        users.Remove(existingUser);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? existingUser = users.SingleOrDefault(x => x.Id == id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with id {id} does not exist");
        }

        return Task.FromResult(existingUser);
    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }
}