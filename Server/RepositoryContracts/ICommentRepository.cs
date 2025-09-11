using Entities;

namespace RepositoryContracts;

public interface ICommentRepository
{
    Task<Comment> CreateAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(Comment comment);
    Task<Comment> GetSingleAsync(int id);
    IQueryable<Comment> GetManyAsync();
}