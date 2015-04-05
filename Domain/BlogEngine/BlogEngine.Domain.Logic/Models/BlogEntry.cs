using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogEngine.Domain.Models
{
    public class BlogEntry
    {
        public BlogEntry()
        {
            Tags = new List<Tag>();
        }

        public int Id { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public string Markdown { get; set; }
        public string Summary { get; set; }
        public bool? IsPublished { get; set; }
        public bool? IsCodePrettified { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class BlogEntryModel
    {
        public BlogEntryModel()
        {
            Tags = new List<string>();
        }

        public string Slug { get; set; }
        public string NewSlug { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
        public string Summary { get; set; }
        public bool IsCodePrettified { get; set; }
        public List<string> Tags { get; set; }
        public string Markdown { get; set; }
        public bool? IsPublished { get; set; }

        public string TagsString { get; set; }

        public void RebuildTagsFromString()
        {
            if (!string.IsNullOrWhiteSpace(TagsString))
            {
                Tags = TagsString.Split(' ').ToList();
            }
        }

        public void BuildTagStringFromList()
        {
            TagsString = string.Join(" ", Tags);            
        }
    }

    public class BlogEntrySummaryModel
    {
        public BlogEntrySummaryModel()
        {
            Tags = new List<string>();
        }

        public string Key { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Date { get; set; }
        public string PrettyDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Tags { get; set; }
    }
}
