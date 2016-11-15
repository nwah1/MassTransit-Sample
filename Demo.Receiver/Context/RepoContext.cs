using System.Data.Entity;
using Pangea.Messaging;

namespace Demo.Receiver.Context
{
    public class RepoContext : DbContext
    {
        public DbSet<Repo> Repos { get; set; }
    }
}