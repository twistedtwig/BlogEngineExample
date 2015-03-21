using System;
using System.Collections.Generic;

namespace BlogEngine.Repository.Models
{
    public class BlogEntryEntity
    {
        public BlogEntryEntity()
        {
            Tags = new List<TagEntity>();
        }

        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public string Markdown { get; set; }
        public bool? IsPublished { get; set; }
        public bool? IsCodePrettified { get; set; }

        public List<TagEntity> Tags { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
