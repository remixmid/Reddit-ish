using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<ICollection<CommentDto>> GetByPostIdAsync(int postId)
    {
        HttpResponseMessage response = await client.GetAsync($"Comments/post/{postId}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<ICollection<CommentDto>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<CommentDto> AddAsync(CommentCreateDto request)
    {
        var response = await client.PostAsJsonAsync("Comments", request);
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        return JsonSerializer.Deserialize<CommentDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}
