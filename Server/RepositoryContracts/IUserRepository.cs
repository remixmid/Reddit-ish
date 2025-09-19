using Entities;

namespace RepositoryContracts;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<User> GetSingleAsync(int id);
    Task<User> GetSingleAsync(string username);
    IQueryable<User> GetManyAsync();
}