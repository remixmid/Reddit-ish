using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<ICollection<PostDto>> GetAllAsync();
    Task<PostDto> GetByIdAsync(int id);
    Task<PostDto> CreateAsync(PostCreateDto dto);

    Task DeleteAsync(int postId, int userId);
    Task UpdateAsync(int postId, PostUpdateDto dto, int userId);
}
