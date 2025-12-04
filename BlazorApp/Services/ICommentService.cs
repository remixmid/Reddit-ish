using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<ICollection<CommentDto>> GetByPostIdAsync(int postId);
    Task<CommentDto> AddAsync(CommentCreateDto dto);
}
