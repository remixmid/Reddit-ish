using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository  : ICommentRepository
{
    List<Comment> _comments = new List<Comment>();
    
    public Task<Comment> CreateAsync(Comment comment)
    {
        int maxId = 1;
        foreach (var item in _comments)
        {
            if (item.Id > maxId)
            {
                maxId = item.Id;
            }
        }
        comment.Id = maxId + 1;
        _comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        foreach (var item in _comments)
        {
            if (item.Id == comment.Id)
            {
                _comments.Remove(item);
                _comments.Add(comment);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"Comment with id {comment.Id} was not found"
        );
    }

    public Task DeleteAsync(Comment comment)
    {
        foreach (var item in _comments)
        {
            if (item.Id == comment.Id)
            {
                _comments.Remove(item);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"Comment with id {comment.Id} was not found"
        );
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        foreach (var item in _comments)
        {
            if (item.Id == id)
            {
                return Task.FromResult(item);
            }
        }

        throw new InvalidOperationException(
            $"Comment with id {id} was not found"
        );
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return _comments.AsQueryable();
    }
}