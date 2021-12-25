using System.Data.Entity;

namespace Model
{
    public class Context : DbContext
    {
        public Context() : base("DefaultConnection")
        {
            Database.SetInitializer<Context>(new CreateDatabaseIfNotExists<Context>());
            //Database.SetInitializer<Context>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
