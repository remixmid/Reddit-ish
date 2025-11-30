using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly IUserRepository userRepo;
    private CreateUserView createUserView;
    private ListUsersView listUsersView;

    public ManageUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
        listUsersView = new ListUsersView(userRepo);
        createUserView = new CreateUserView(userRepo);
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.WriteLine("\n-- USERS --");
            Console.WriteLine("1) Create user");
            Console.WriteLine("2) List users");
            Console.WriteLine("0) Back");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": await createUserView.CreateUserAsync(); break;
                case "2": await listUsersView.ListUsersAsync(); break;
                case "0": return;
            }
        }
    }
}