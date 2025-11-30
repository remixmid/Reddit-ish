using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    public List<Comment> comments { get; } = new();

    public CommentInMemoryRepository()
    {
        comments.Add(new Comment(1, 4, "FirstTestCommentBody", 3));
        comments.Add(new Comment(2, 1, "SecondTestCommentBody", 1));
        comments.Add(new Comment(3, 5, "ThirdTestCommentBody", 1));
        comments.Add(new Comment(4, 3, "FourthTestCommentBody",1));
        comments.Add(new Comment(5, 2, "FifthTestCommentBody", 4));
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any() ? comments.Max(x => x.Id) + 1 : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(x => x.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with id {comment.Id} was not found.");
        }

        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? existingComment = comments.SingleOrDefault(x => x.Id == id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with id {id} was not found.");
        }

        comments.Remove(existingComment);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? existingComment = comments.SingleOrDefault(x => x.Id == id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with id {id} was not found.");
        }

        return Task.FromResult(existingComment);
    }

    public IQueryable<Comment> GetManyAsync(int postId)
    {
        List<Comment> postComments = new List<Comment>();
        foreach (Comment comment in comments)
        {
            if (comment.PostId == postId)
            {
                postComments.Add(comment);
            }
        }

        return postComments.AsQueryable();
    }
}