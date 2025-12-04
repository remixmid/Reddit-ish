namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;

    public int UserId { get; set; }  
    public User User { get; set; } = null!;

    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    public Post()
    {
    }

    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    }

    public Post(int id, string title, string body, int userId)
    {
        Id = id;
        Title = title;
        Body = body;
        UserId = userId;
    }
}