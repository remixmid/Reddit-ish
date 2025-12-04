using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<ICollection<CommentDto>> GetByPostIdAsync(int postId);
    Task<CommentDto> AddAsync(CommentCreateDto dto);

    Task DeleteAsync(int commentId, int userId);
    Task UpdateAsync(int commentId, CommentUpdateDto dto, int userId);
}
