namespace EfcRepositories;

using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using AppDbContext = EfcRepositories.AppContext;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext _appContext;
    
    public EfcPostRepository(AppContext appContext)
    {
        _appContext = appContext;
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        await _appContext.Posts.AddAsync(post);
        await _appContext.SaveChangesAsync();
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        if (!(await _appContext.Posts.AnyAsync(p => p.Id == post.Id)))
        {
            throw new Exception("Post with id {post.Id} not found");
        } 
        _appContext.Posts.Update(post); 
        await _appContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await _appContext.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} not found");
        } 
        _appContext.Posts.Remove(existing); 
        await _appContext.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        Post? existing = await _appContext.Posts.SingleOrDefaultAsync(p => p.Id == id);
        return existing;
    }

    public async Task<Post> GetSingleAsync(string title)
    {
        Post? existing = await _appContext.Posts.SingleOrDefaultAsync(p => p.Title == title);
        return existing;
    }


    public IQueryable<Post> GetManyAsync()
    {
        return _appContext.Posts.AsQueryable();
    }
}
