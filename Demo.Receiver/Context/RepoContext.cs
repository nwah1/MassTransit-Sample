using System.Data.Entity;
using Demo.Model;

namespace Demo.Receiver.Context
{
    public class RepoContext : DbContext
    {
        public DbSet<Repo> Repos { get; set; }
    }
}