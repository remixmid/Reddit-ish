using ApiContracts;

namespace BlazorApp.Auth;

using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using LoginRequest = ApiContracts.Requests.LoginRequest;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    
    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string userAsJson = "";
        try
        {
            userAsJson = await _jsRuntime.InvokeAsync<string>(
                "sessionStorage.getItem", "currentUser");
        }
        catch (InvalidOperationException)
        {
            return new AuthenticationState(new());
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new());
        }
        
        UserDto userDto = JsonSerializer.Deserialize<UserDto>(userAsJson)!;
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth"); 
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity); 
        return new AuthenticationState(claimsPrincipal);
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            "auth/login",
            new LoginRequest
            {
                UserName = userName,
                Password = password
            });
        
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        string serialisedData = JsonSerializer.Serialize(userDto);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);
        
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", ""); 
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }
}