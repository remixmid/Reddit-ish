using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class SubForumInMemoryRepository : ISubForumRepository
{
    List<SubForum> _subForums = new List<SubForum>();
    
    public Task<SubForum> AddSubForumAsync(SubForum subForum)
    {
        int maxId = 1;
        foreach (var item in _subForums)
        {
            if (item.Id > maxId)
            {
                maxId = item.Id;
            }
        }
        subForum.Id = maxId + 1;
        _subForums.Add(subForum);
        return Task.FromResult(subForum);
    }

    public Task UpdateSubForumAsync(SubForum subForum)
    {
        foreach (var item in _subForums)
        {
            if (item.Id == subForum.Id)
            {
                _subForums.Remove(item);
                _subForums.Add(subForum);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"SubForum with id {subForum.Id} was not found"
        );
    }

    public Task DeleteSubForumAsync(SubForum subForum)
    {
        foreach (var item in _subForums)
        {
            if (item.Id == subForum.Id)
            {
                _subForums.Remove(item);
                return Task.CompletedTask;
            }
        }

        throw new InvalidOperationException(
            $"SubForum with id {subForum.Id} was not found"
        );
    }

    public Task<SubForum> GetSingleAsync(int id)
    {
        foreach (var item in _subForums)
        {
            if (item.Id == id)
            {
                return Task.FromResult(item);
            }
        }

        throw new InvalidOperationException(
            $"SubForum with id {id} was not found"
        );
    }

    public IQueryable<SubForum> GetManyAsync()
    {
        return _subForums.AsQueryable();
    }
}