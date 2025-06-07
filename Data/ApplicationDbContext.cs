using Microsoft.EntityFrameworkCore;
using EvaluadorInteligente.Models;

namespace EvaluadorInteligente.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt) { }

        public DbSet<Product> Products   => Set<Product>();
        public DbSet<User>    Users      => Set<User>();
        public DbSet<Rating>  Ratings    => Set<Rating>();
    }
}
