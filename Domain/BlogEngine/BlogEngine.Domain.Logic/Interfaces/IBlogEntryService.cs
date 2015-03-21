using BlogEngine.Domain.Models;
using General.Requests;

namespace BlogEngine.Domain.Interfaces
{
    public interface IBlogEntryService
    {
        /// <summary>
        /// creates or updates the blog entry.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>Service result with the slug as the ID</returns>
        ServiceResult<string> Save(BlogEntryModel entry);
        BlogEntryModel GetBySlug(string slug);
        BlogEntrySummaryModel[] GetList(bool onlyPublished = true);
        BlogEntrySummaryModel[] GetList(params string[] tags);
        void Delete(string slug);
        bool Exists(string slug);
    }
}
