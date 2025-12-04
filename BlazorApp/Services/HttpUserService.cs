using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var response = await client.PostAsJsonAsync("users", dto);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var created = JsonSerializer.Deserialize<UserDto>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return created!;
    }
}
