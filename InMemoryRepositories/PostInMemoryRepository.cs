using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> posts { get; } = new();

    public PostInMemoryRepository()
    {
        posts.Add(new Post(1, "FirstTestTitle", "First test body" +
                                                " \nFirst test body" + 
                                                " \nFirst test body", 2));
        posts.Add(new Post(2, "SecondTestTitle", "Second test body", 1));
        posts.Add(new Post(3, "ThirdTestTitle", "Third test body", 5));
        posts.Add(new Post(4, "FourthTestTitle", "Fourth test body", 3));
        posts.Add(new Post(5, "FifthTestTitle", "Fifth test body", 4));
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any() ? posts.Max(x => x.Id) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(x => x.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"The Post with id {post.Id} was not found.");
        }

        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? existingPost = posts.SingleOrDefault(x => x.Id == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"The Post with id {id} was not found.");
        }

        posts.Remove(existingPost);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? existingPost = posts.SingleOrDefault(x => x.Id == id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"The Post with id {id} was not found.");
        }

        return Task.FromResult(existingPost);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
}