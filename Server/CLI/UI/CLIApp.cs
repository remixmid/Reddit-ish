using RepositoryContracts;

namespace CLI.UI;

public class CLIApp
{
    public readonly IPostRepository postRepository;
    public readonly ICommentRepository commentRepository;
    public readonly IUserRepository userRepository;
    public readonly ISubForumRepository subForumRepository;

    public CLIApp(IPostRepository postRepository, ICommentRepository commentRepository,
        ISubForumRepository subForumRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.subForumRepository = subForumRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Welcome to the CLI app!\n" +
                          "Please login with your account and press ENTER.\n" +
                          "Please enter your Username and press ENTER.");
        string userName = Console.ReadLine();
        if (userRepository.GetSingleAsync(userName) != null)
        {
            Console.WriteLine("Please enter your Password and press ENTER.");
            if (userRepository.GetSingleAsync(userName).Result.Password == Console.ReadLine())
            {
                Console.WriteLine("Welcome to system.");
            } 
            
        }
    }

}