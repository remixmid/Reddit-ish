using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    List<Post> _posts = new List<Post>();
    
    public Task<Post> AddAsync(Post post)
    {
        int maxId = 1;
        foreach (var item in _posts)
        {
            if (item.Id > maxId)
            {
                maxId = item.Id;
            }
        }
        post.Id = maxId + 1;
        _posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        foreach (var item in _posts)
        {
            if (item.Id == post.Id)
            {
                _posts.Remove(item);
                _posts.Add(post);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"Post with id {post.Id} was not found"
        );
    }

    public Task DeleteAsync(Post post)
    {
        foreach (var item in _posts)
        {
            if (item.Id == post.Id)
            {
                _posts.Remove(item);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"Post with id {post.Id} was not found"
        );
    }

    public Task<Post> GetSingleAsync(int id)
    {
        foreach (var item in _posts)
        {
            if (item.Id == id)
            {
                return Task.FromResult(item);
            }
        }

        throw new InvalidOperationException(
            $"Post with id {id} was not found"
        );
    }

    public IQueryable<Post> GetManyAsync()
    {
        return _posts.AsQueryable();
    }
}