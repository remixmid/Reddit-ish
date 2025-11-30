using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;

    public CreatePostView(IPostRepository _postRepo, IUserRepository _userRepo)
    {
        this._postRepo = _postRepo;
        this._userRepo = _userRepo;
    }
    
    public async Task CreatePostAsync()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Body: ");
        string body = Console.ReadLine();

        Console.Write("User ID: ");
        int userId = int.Parse(Console.ReadLine());

        User author = await _userRepo.GetSingleAsync(userId);

        Post post = new Post(title, body, author.Id);
        await _postRepo.AddAsync(post);

        Console.WriteLine($"Post created with id {post.Id}");
    }
}