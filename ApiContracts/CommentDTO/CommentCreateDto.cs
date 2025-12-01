namespace ApiContracts;

public class CommentCreateDto
{
    public int UserId { get; set; }
    public int PostId { get; set; }
    public string Body { get; set; }
}