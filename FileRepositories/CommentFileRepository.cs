using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string _filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 1;
        comment.Id = maxId + 1;
        comments.Add(comment);
        commentAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(_filePath, commentAsJson);
        return comment;
    }

    public Task UpdateAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Comment> GetManyAsync(int postId)
    {
        string commentsAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        List<Comment> postComments = new List<Comment>();
        foreach (Comment comment in comments)
        {
            if (comment.PostId == postId)
            {
                postComments.Add(comment);
            }
        }

        return comments.AsQueryable();
    }
}