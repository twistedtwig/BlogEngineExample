using BlogEngine.Domain.Models;

namespace BlogEngine.Domain.Interfaces
{
    public interface IBlogTagService
    {
        Tag[] GetPossibles(string text);
        TagCount[] GetCount();
        void UpdateTagCount();
        void RemoveUnsedTags();
    }
}