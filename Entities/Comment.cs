namespace Entities;

public class Comment
{
    public User User { get; set; }
    public Post Post { get; set; }
    public string Body { get; set; }
    public int ForeignCommentId { get; set; }
    public int Id { get; set; }
}