using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> CreateAsync(CreateUserDto dto);
}
