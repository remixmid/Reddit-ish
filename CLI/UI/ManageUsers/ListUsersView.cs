using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepo;

    public ListUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task ListUsersAsync()
    {
        var users = userRepo.GetManyAsync();
        foreach (var u in users)
            Console.WriteLine($"{u.Id}: {u.UserName}");
    }
}