using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private readonly ICommentRepository _commentRepo;
    private ListPostsView listPostsView;
    private CreatePostView createPostView;

    public ManagePostsView(
        IPostRepository postRepo,
        IUserRepository userRepo,
        ICommentRepository commentRepo)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
        _commentRepo = commentRepo;
        
        listPostsView = new ListPostsView(_postRepo, _userRepo, _commentRepo);
        createPostView = new CreatePostView(_postRepo, _userRepo);
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine("\n---- POSTS ----");
            Console.WriteLine("1) Create post");
            Console.WriteLine("2) List posts");
            Console.WriteLine("0) Back");
            Console.Write("Choose: ");

            switch (Console.ReadLine())
            {
                case "1": await createPostView.CreatePostAsync(); break;
                case "2": await listPostsView.ListPostsAsync(); break;
                case "0": return;
            }
        }
    }
}
