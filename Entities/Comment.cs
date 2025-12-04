namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int PostId { get; set; }
    public Post Post { get; set; } = null!;
    public Comment()
    {
    }

    public Comment(int userId, string body, int postId)
    {
        PostId = postId;
        UserId = userId;
        Body = body;
    }

    public Comment(int id, int userId, string body,  int postId)
    {
        PostId = postId;
        Id = id;
        UserId = userId;
        Body = body;
    }
}