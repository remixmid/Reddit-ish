using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ISubForumRepository subForumRepository = new SubForumInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

CLIApp cliApp = new CLIApp(postRepository, commentRepository, subForumRepository, userRepository);
await cliApp.StartAsync();