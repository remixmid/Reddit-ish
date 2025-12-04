namespace EfcRepositories;

using Entities;
using Microsoft.EntityFrameworkCore;

public class AppContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    
    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
    }
}
