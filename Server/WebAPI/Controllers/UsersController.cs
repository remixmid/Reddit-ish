using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.UserName);

        User user = new(request.UserName, request.Password);
        User created = await userRepository.AddAsync(user);

        UserDto dto = new()
        {
            Id = created.Id,
            UserName = created.UserName
        };
        return Created($"/users/{dto.Id}", dto);
    }


    [HttpPatch("{id:int}")]
    public async Task<ActionResult<UpdateUserDto>> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        User? existing = await userRepository.GetSingleAsync(id);

        if (existing == null)
            return NotFound();

        if (request.UserName != existing.UserName)
            await EnsureUserNameUniqueAsync(request.UserName);

        existing.UserName = request.UserName;

        if (!string.IsNullOrWhiteSpace(request.Password))
            existing.Password = request.Password;

        await userRepository.UpdateAsync(existing);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetMany()
    {
        var users = userRepository.GetManyAsync();

        var dtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName
        });

        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle([FromRoute] int id)
    {
        User? user = await userRepository.GetSingleAsync(id);
        if (user == null)
            return NotFound();
        return Ok(new UserDto { Id = user.Id, UserName = user.UserName });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        User? user = await userRepository.GetSingleAsync(id);
        if (user == null)
            return NotFound();
        await userRepository.DeleteAsync(id);
        return NoContent();
    }

    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        bool exists = await userRepository
            .GetManyAsync()
            .AnyAsync(u => u.UserName == userName);

        if (exists)
            throw new Exception("Username already exists");
    }

    private async Task EnsureUserNameUniqueAsync(string userName)
    {
        bool exists = await userRepository
            .GetManyAsync()
            .AnyAsync(u => u.UserName == userName);

        if (exists)
            throw new Exception("Username already taken");
    }
}