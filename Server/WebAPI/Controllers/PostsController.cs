using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public PostsController(
        IPostRepository postRepository,
        IUserRepository userRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create([FromBody] PostCreateDto request)
    {
        var user = await userRepository.GetSingleAsync(request.UserId);
        if (user == null)
            return NotFound($"User with id {request.UserId} was not found.");

        Post post = new(request.Title, request.Body, request.UserId);
        Post created = await postRepository.AddAsync(post);

        PostDto dto = new()
        {
            Id = created.Id,
            Title = created.Title,
            Body = created.Body,
            UserId = created.UserId
        };

        return Created($"/Posts/{dto.Id}", dto);
    }


    [HttpPatch("{id:int}")]
    public async Task<ActionResult<PostUpdateDto>> UpdatePost([FromRoute] int id, [FromBody] PostUpdateDto request)
    {
        Post? existing = await postRepository.GetSingleAsync(id);

        if (existing == null)
            return NotFound();
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            existing.Title = request.Title;
            existing.Body = request.Body;
        }

        await postRepository.UpdateAsync(existing);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetMany()
    {
        var posts = await postRepository.GetManyAsync().ToListAsync();

        var dtos = posts.Select(p => new PostDto
        {
            Id = p.Id,
            Title = p.Title,
            Body = p.Body,
            UserId = p.UserId
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle([FromRoute] int id)
    {
        Post? post = await postRepository.GetSingleAsync(id);
        if (post == null)
            return NotFound();
        return Ok(new PostDto { Id = post.Id, Title = post.Title, Body = post.Body, UserId = post.UserId });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        Post? post = await postRepository.GetSingleAsync(id);
        if (post == null)
            return NotFound();
        await postRepository.DeleteAsync(id);
        return NoContent();
    }
}