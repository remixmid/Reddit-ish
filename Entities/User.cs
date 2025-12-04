namespace Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public ICollection<Post> Posts { get; private set; } = new List<Post>();
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    public User()
    {
    }

    public User(string username, string password)
    {
        UserName = username;
        Password = password;
    }

    public User(int id, string username, string password)
    {
        Id = id;
        UserName = username;
        Password = password;
    }
}