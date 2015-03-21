using System.Data.Entity.ModelConfiguration;
using BlogEngine.Repository.Models;

namespace BlogEngine.Repository.Configuration
{
    public class TagCountEntityConfiguration : EntityTypeConfiguration<TagCountEntity>
    {
        public TagCountEntityConfiguration()
        {
            HasKey(a => a.Id).ToTable("TagCount", "Blog");
        }
    }
}