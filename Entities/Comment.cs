namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Body { get; set; }

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