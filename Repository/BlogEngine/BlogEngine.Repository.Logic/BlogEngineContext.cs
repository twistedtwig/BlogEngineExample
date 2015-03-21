using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace BlogEngine.Repository
{
    /// <summary>
    /// the dbcontext for the blog engine.  configuration is done fluently in the configuration folder.  All access is controlled via the BlogRepository.
    /// </summary>
    public class BlogEngineContext : DbContext
    {
        public BlogEngineContext() : base("blogEngine")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetAssembly(typeof(BlogEngineContext)));            
        }
    }
}
