using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<ICollection<PostDto>> GetAllAsync()
    {
        var response = await client.GetAsync("posts");
        response.EnsureSuccessStatusCode();
        var posts = await response.Content.ReadFromJsonAsync<List<PostDto>>();
        return posts ?? new List<PostDto>();
    }

    public async Task<PostDto> GetByIdAsync(int id)
    {
        var response = await client.GetAsync($"posts/{id}");
        response.EnsureSuccessStatusCode();
        var post = await response.Content.ReadFromJsonAsync<PostDto>();
        return post!;
    }

    public async Task<PostDto> CreateAsync(PostCreateDto dto)
    {
        var response = await client.PostAsJsonAsync("posts", dto);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        var created = JsonSerializer.Deserialize<PostDto>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return created!;
    }
}
