using System.Data.Entity.ModelConfiguration;
using BlogEngine.Repository.Models;

namespace BlogEngine.Repository.Configuration
{
    public class TagEntityConfiguration : EntityTypeConfiguration<TagEntity>
    {
        public TagEntityConfiguration()
        {
            HasKey(a => a.Id).ToTable("Tags", "Blog");
        }
    }
}