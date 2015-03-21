using System.Data.Entity.ModelConfiguration;
using BlogEngine.Repository.Models;

namespace BlogEngine.Repository.Configuration
{
    public class BlogUserEntityConfiguration : EntityTypeConfiguration<BlogUserEntity>
    {
        public BlogUserEntityConfiguration()
        {
            HasKey(x => x.Id).ToTable("BlogUsers", "Blog");
        }
    }
}