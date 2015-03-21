

namespace BlogEngine.Repository.Models
{
    /// <summary>
    /// Represents an individual tag
    /// </summary>
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents the number of times a tag has been used.
    /// </summary>
    public class TagCountEntity
    {
        public int Id { get; set; }
        public TagEntity Tag { get; set; }
        public int Count { get; set; }
    }
}
