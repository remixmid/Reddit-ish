namespace Entities;

public class Post
{
    public User User { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int Id { get; set; }
}