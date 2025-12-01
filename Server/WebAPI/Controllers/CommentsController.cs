using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CommentsController(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }
    
    [HttpPost]
    public async Task<ActionResult<CommentDto>> CreateComment([FromBody] CommentCreateDto request)
    {
        var user = await userRepository.GetSingleAsync(request.UserId);
        if (user == null)
            return NotFound($"User with id {request.UserId} not found.");

        var post = await postRepository.GetSingleAsync(request.PostId);
        if (post == null)
            return NotFound($"Post with id {request.PostId} not found.");

        Comment comment = new(request.UserId, request.Body, request.PostId);
        Comment created = await commentRepository.AddAsync(comment);

        CommentDto dto = new()
        {
            Id = created.Id,
            UserId = created.UserId,
            PostId = created.PostId,
            Body = created.Body
        };

        return Created($"/comments/{dto.Id}", dto);
    }
    
    [HttpGet("/posts/{postId:int}/comments")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> GetByPostId([FromRoute] int postId)
    {
        var post = await postRepository.GetSingleAsync(postId);
        if (post == null)
            return NotFound($"Post with id {postId} not found.");

        var comments = commentRepository.GetManyAsync(postId);

        var filtered = comments
            .Where(c => c.PostId == postId)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                UserId = c.UserId,
                PostId = c.PostId,
                Body = c.Body
            });

        return Ok(filtered);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle([FromRoute] int id)
    {
        var c = await commentRepository.GetSingleAsync(id);
        if (c == null) return NotFound();

        CommentDto dto = new()
        {
            Id = c.Id,
            UserId = c.UserId,
            PostId = c.PostId,
            Body = c.Body
        };

        return Ok(dto);
    }
    
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateComment(
        [FromRoute] int id,
        [FromBody] CommentUpdateDto request)
    {
        var existing = await commentRepository.GetSingleAsync(id);
        if (existing == null)
            return NotFound();

        existing.Body = request.Body;

        await commentRepository.UpdateAsync(existing);

        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        var existing = await commentRepository.GetSingleAsync(id);
        if (existing == null)
            return NotFound();

        await commentRepository.DeleteAsync(id);
        return NoContent();
    }
}