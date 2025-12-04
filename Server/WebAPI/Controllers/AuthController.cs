using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto request)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName);

        if (user is null)
            return Unauthorized("Invalid username or password");

        if (user.Password != request.Password)
            return Unauthorized("Invalid username or password");

        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Ok(userDto);
    }
}
