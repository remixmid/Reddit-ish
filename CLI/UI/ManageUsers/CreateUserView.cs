using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepo;

    public CreateUserView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task CreateUserAsync()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine();

        Console.Write("Password: ");
        string password = Console.ReadLine();

        User u = new User(username, password);
        await userRepo.AddAsync(u);

        Console.WriteLine($"User created with id {u.Id}");
    }
}