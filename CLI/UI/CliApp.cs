using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    public IUserRepository userRepository { get; }
    public ICommentRepository commentRepository { get; }
    public IPostRepository postRepository { get; }

    public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- MAIN MENU ---");
            Console.WriteLine("1) Manage Users");
            Console.WriteLine("2) Manage Posts");
            Console.WriteLine("3) Manage Comments");
            Console.WriteLine("0) Exit");

            Console.Write("Choose: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": await new ManageUsersView(userRepository).RunAsync(); break;
                case "2": await new ManagePostsView(postRepository, userRepository, commentRepository).RunAsync(); break;
                case "0": return;
            }
        }
    }
}