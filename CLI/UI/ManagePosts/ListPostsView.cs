using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private readonly ICommentRepository _commentRepo;
    private SinglePostView _singlePostView;

    public ListPostsView(IPostRepository _postRepo, IUserRepository _userRepo, ICommentRepository _commentRepo)
    {
        this._postRepo = _postRepo;
        this._userRepo = _userRepo;
        this._commentRepo = _commentRepo;
        _singlePostView = new SinglePostView(_postRepo, _userRepo, _commentRepo);
    }

    public async Task ListPostsAsync()
    {
        var posts = _postRepo.GetManyAsync().ToList();
        Console.WriteLine("Choose a post:");
        foreach (var p in posts)
            Console.WriteLine($"[{p.Id}] {p.Title}");
        try
        {
            _singlePostView.ViewPostAsync(int.Parse(Console.ReadLine()));
        }
        catch (Exception e)
        {
            ListPostsAsync();
        }
            
    }
}