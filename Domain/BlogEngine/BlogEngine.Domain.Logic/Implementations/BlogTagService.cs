using System.Linq;
using BlogEngine.Domain.Interfaces;
using BlogEngine.Domain.Models;
using BlogEngine.Repository.Interfaces;
using BlogEngine.Repository.Models;

namespace BlogEngine.Domain.Implementations
{
    public class BlogTagService : IBlogTagService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogTagService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public TagCount[] GetCount()
        {
            return _blogRepository.All<TagCountEntity>().Select(AutoMapper.Mapper.Map<TagCountEntity, TagCount>).ToArray();
        }

        public Tag[] GetPossibles(string text)
        {
            return _blogRepository.List<TagEntity>(t => t.Name.Contains(text)).Select(AutoMapper.Mapper.Map<TagEntity, Tag>).ToArray();
        }

        public void UpdateTagCount()
        {
            _blogRepository.UpdateTagCount();
        }

        public void RemoveUnsedTags()
        {
            _blogRepository.RemoveUnusedTags();
        }
    }
}