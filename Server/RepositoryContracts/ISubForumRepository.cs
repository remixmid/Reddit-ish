using Entities;

namespace RepositoryContracts;

public interface ISubForumRepository
{
    Task<SubForum> AddSubForumAsync(SubForum subForum);
    Task UpdateSubForumAsync(SubForum subForum);
    Task DeleteSubForumAsync(SubForum subForum);
    Task<SubForum> GetSingleAsync(int id);
    IQueryable<SubForum> GetManyAsync();
    
}