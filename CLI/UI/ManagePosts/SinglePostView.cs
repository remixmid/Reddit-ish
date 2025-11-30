using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository _postRepo;
    private readonly IUserRepository _userRepo;
    private readonly ICommentRepository _commentRepo;

    public SinglePostView(IPostRepository postRepo, IUserRepository userRepo, ICommentRepository commentRepo)
    {
        _postRepo = postRepo;
        _userRepo = userRepo;
        _commentRepo = commentRepo;
    }


    public async Task ViewPostAsync(int id)
    {
        Post post = await _postRepo.GetSingleAsync(id);

        Console.WriteLine("Title: " + post.Title + "\n\nBody: " + post.Body + "\n\nComments:");

        foreach (var c in _commentRepo.GetManyAsync(id).Where(c => c.PostId == id))
        {
            var user = await _userRepo.GetSingleAsync(c.UserId);
            Console.WriteLine($"{user.Username}: {c.Body}");
        }

        Console.WriteLine("\n8 - Update post");
        Console.WriteLine("9 - Delete post");
        Console.WriteLine("0 - Back");

        switch (Console.ReadLine())
        {
            case "8":
                Console.WriteLine("Enter new body:");
                string newBody = Console.ReadLine();

                // create updated post
                Post updated = new Post(post.Id, post.Title, newBody, post.UserId);

                await _postRepo.UpdateAsync(updated);

                Console.WriteLine("Post updated successfully");
                break;

            case "9":
                await _postRepo.DeleteAsync(id);
                Console.WriteLine("Post deleted");
                break;

            case "0":
                return;
        }
    }

}