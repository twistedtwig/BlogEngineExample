
namespace BlogEngine.Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TagCount
    {        
        public Tag Tag { get; set; }
        public int Count { get; set; }
    }
}
