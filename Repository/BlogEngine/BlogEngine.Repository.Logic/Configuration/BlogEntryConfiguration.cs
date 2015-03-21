using System.Data.Entity.ModelConfiguration;
using BlogEngine.Repository.Models;

namespace BlogEngine.Repository.Configuration
{
    public class BlogEntryEntityConfiguration : EntityTypeConfiguration<BlogEntryEntity>
    {
        public BlogEntryEntityConfiguration()
        {
            HasKey(a => a.Id).ToTable("Entries", "Blog");

            HasMany(e => e.Tags)
                .WithMany()
                .Map(m => m.MapLeftKey("EntryId").MapRightKey("TagId").ToTable("EntryTags", "Blog"));

        }
    }
}
