using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentsController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    // GET /Comments/post/2  -> все комментарии к посту 2
    [HttpGet("post/{postId:int}")]
    public ActionResult<IEnumerable<CommentDto>> GetByPost(int postId)
    {
        var comments = commentRepo
            .GetManyAsync(postId)
            .Where(c => c.PostId == postId)
            .Select(c => new CommentDto
            {
                Id = c.Id,
                Body = c.Body,
                PostId = c.PostId,
                UserId = c.UserId
            })
            .ToList();

        return Ok(comments);
    }

    // создание комментария, если уже есть:
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CommentCreateDto request)
    {
        Comment comment = new(request.UserId, request.Body, request.PostId);
        Comment created = await commentRepo.AddAsync(comment);

        CommentDto dto = new()
        {
            Id = created.Id,
            Body = created.Body,
            PostId = created.PostId,
            UserId = created.UserId
        };

        return Created($"/Comments/{dto.Id}", dto);
    }
}
