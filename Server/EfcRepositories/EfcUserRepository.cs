namespace EfcRepositories;

using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext _appContext;
    
    public EfcUserRepository(AppContext appContext)
    {
        _appContext = appContext;
    }
    
    public async Task<User> AddAsync(User user)
    {
        await _appContext.Users.AddAsync(user);
        await _appContext.SaveChangesAsync();
        return user;
    }

    Task IUserRepository.UpdateAsync(User user)
    {
        return UpdateAsync(user);
    }

    public async Task<User> UpdateAsync(User user)
    {
        if (!(await _appContext.Users.AnyAsync(u => u.Id == user.Id)))
        {
            throw new Exception($"User with id {user.Id} not found");
        } 
        
        _appContext.Users.Update(user);
        await _appContext.SaveChangesAsync();
        return user;
    }

    public Task DeleteAsync(int id)
    {
        User? existing =  _appContext.Users.SingleOrDefault(u => u.Id == id);
        if (existing == null)
        {
            throw new Exception($"User with id {id} not found");
        }
        _appContext.Users.Remove(existing);
        return _appContext.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? existing =  await _appContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        return existing;
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        User? existing =  await _appContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        return existing;
    }

    public IQueryable<User> GetManyAsync()
    {
        return  _appContext.Users.AsQueryable();  
    }
}