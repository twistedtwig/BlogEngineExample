using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using BlogEngine.Domain.Extensions;
using BlogEngine.Domain.Interfaces;
using BlogEngine.Domain.Models;
using BlogEngine.Repository.Interfaces;
using BlogEngine.Repository.Models;
using General;
using General.Requests;
using UserManagementService.Interfaces;

namespace BlogEngine.Domain.Implementations
{
    public class BlogEntryService : IBlogEntryService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IUserService<BlogUser, BlogUserEntity, int> _blogUserService;
        private readonly IBlogTagService _blogTagService;
        private readonly IGetCurrentUserName _currentUserService;
        
        public BlogEntryService(IBlogRepository blogRepository, IBlogTagService blogTagService, IGetCurrentUserName currentUserService, IUserService<BlogUser, BlogUserEntity, int> blogUserService)
        {
            _blogRepository = blogRepository;
            _blogUserService = blogUserService;
            _blogTagService = blogTagService;
            _currentUserService = currentUserService;
        }

        public ServiceResult<string> Save(BlogEntryModel entryModel)
        {
            if (entryModel == null) throw new ArgumentNullException("entryModel");
            entryModel.RebuildTagsFromString();

            BlogEntry entry = null;

            if (!Exists(entryModel.Slug))
            {
                entry = new BlogEntry
                {
                    Slug = entryModel.Title.ToUrlSlug(), 
                    DateCreated = DateTime.Now
                };

                if (Exists(entry.Slug))
                {
                    return ServiceResult<string>.Error("Sorry, a post with that slug already exists.");
                }
            }
            else
            {
                entry = GenericMapper<BlogEntryEntity, BlogEntry>.ToModelWithSubTypes(_blogRepository.Single<BlogEntryEntity>(b => b.Slug == entryModel.Slug));
                entry.DateCreated = DateTime.ParseExact(entryModel.Date, DateSettings.DateStringFormat(), Thread.CurrentThread.CurrentCulture);

                var slugChanged =
                   !string.Equals(entryModel.Slug, entryModel.NewSlug, StringComparison.InvariantCultureIgnoreCase)
                   && !string.IsNullOrWhiteSpace(entryModel.NewSlug);

                if (slugChanged)
                {
                    if (Exists(entryModel.NewSlug))
                    {
                        return ServiceResult<string>.Error("Sorry, a post with that slug already exists.");
                    }
                    //delete current one as slug is the ID will create / save a new one.
                    Delete(entryModel.Slug);
                    entry.Slug = entryModel.NewSlug.ToLowerInvariant();
                }
            }
            
            if (!IsValidSlug(entry.Slug))
            {
                return ServiceResult<string>.Error("That's not a valid slug. Only letters, numbers and hypens are allowed.");
            }
            
            entry.Title = entryModel.Title;            
            entry.Markdown = entryModel.Markdown;
            entry.Summary = CreateSummary(entryModel.Markdown);
            entry.IsPublished = entryModel.IsPublished;
            entry.IsCodePrettified = entryModel.IsCodePrettified;

            entry.Author = _blogUserService.FindByName(_currentUserService.GetUserName()).DisplayName;
            
            //update all the tags.
            var tags = _blogRepository.All<TagEntity>().ToArray();

            var distinctTags = entryModel.Tags.Distinct();
            var trimmedTags = distinctTags.Select(t => t.Trim().ToLowerInvariant()).ToArray();
            var matchedTags = tags.Where(t => trimmedTags.Contains(t.Name.ToLowerInvariant())).ToArray();

            var newTags = trimmedTags.Where(t => tags.All(tag => tag.Name.ToLowerInvariant() != t))
                .Select(t => new TagEntity {Id = 0, Name = t}).ToArray();

            //ensure new tags are added to the list of known tags.
            foreach (var newTag in newTags)
            {
                _blogRepository.Add(newTag);
            }
             
            var actualTags = newTags.Union(matchedTags);

            var entryEntity = GenericMapper<BlogEntryEntity, BlogEntry>.ToEntity(entry);
            entryEntity.Tags.Clear();
            entryEntity.Tags.AddRange(actualTags);

            _blogRepository.Save(entryEntity, b => b.Slug == entry.Slug);

            _blogTagService.RemoveUnsedTags();
            _blogTagService.UpdateTagCount();

            return ServiceResult<string>.Success(entry.Slug);
        }

        private bool IsValidSlug(string slug)
        {
            return Regex.IsMatch(slug, "^[a-zA-Z0-9-]+$");
        }

        public BlogEntryModel GetBySlug(string slug)
        {
            return MapToModel(GenericMapper<BlogEntryEntity, BlogEntry>.ToModelWithSubTypes(_blogRepository.Single<BlogEntryEntity>(b => b.Slug == slug)));
        }

        public BlogEntrySummaryModel[] GetList(bool onlyPublished = true)
        {
           var list = onlyPublished
                ? _blogRepository.List<BlogEntryEntity>(b => b.IsPublished.HasValue && b.IsPublished.Value && b.DateCreated <= DateTime.Now)
                    .Select(GenericMapper<BlogEntryEntity, BlogEntry>.ToModelWithSubTypes)
                    .ToList() 
                : _blogRepository.All<BlogEntryEntity>().Select(GenericMapper<BlogEntryEntity, BlogEntry>.ToModelWithSubTypes).ToList();

           return list.OrderByDescending(e => e.DateCreated).Select(MapToSummary).ToArray();
        }

        public BlogEntrySummaryModel[] GetList(params string[] tags)
        {
            return _blogRepository.ListByTags(tags)
                .Select(GenericMapper<BlogEntryEntity, BlogEntry>.ToModelWithSubTypes)
                .OrderByDescending(e => e.DateCreated)
                .Select(MapToSummary)
                .ToArray();
        }

        public void Delete(string slug)
        {
            _blogRepository.Delete<BlogEntryEntity>(b => b.Slug == slug);
        }

        public bool Exists(string slug)
        {
            return _blogRepository.Exists<BlogEntryEntity>(b => b.Slug == slug);
        }

        private BlogEntryModel MapToModel(BlogEntry entry)
        {
            var html = ConvertToMarkDown(entry.Markdown);
            var summary = ConvertToMarkDown(entry.Summary);
            
            var model = new BlogEntryModel
            {
                Date = entry.DateCreated.ToString(DateSettings.DateStringFormat()),
                Slug = entry.Slug,
                NewSlug = entry.Slug,
                Title = entry.Title,
                Html = html,
                Summary = summary,
                Markdown = entry.Markdown,
                Tags = entry.Tags.Select(t => t.Name).ToList(),
                IsCodePrettified = entry.IsCodePrettified ?? true,
                IsPublished = entry.IsPublished
            };

            model.BuildTagStringFromList();
            return model;
        }

        private string CreateSummary(string text)
        {
            const int summaryLength = 300;

            var summary = string.Empty;
            if (!string.IsNullOrWhiteSpace(text))
            {
                summary = text.Trim().Length <= summaryLength ? text : text.Substring(0, summaryLength) + "...";
            }

            return summary;
        }

        private string ConvertToMarkDown(string text)
        {
            var markdown = new MarkdownSharp.Markdown();
            return markdown.Transform(text);
        }

        private BlogEntrySummaryModel MapToSummary(BlogEntry entry)
        {
            return new BlogEntrySummaryModel
            {
                Key = entry.Slug,
                Title = entry.Title,
                Summary = ConvertToMarkDown(entry.Summary),
                Date = entry.DateCreated.ToDateString(),
                PrettyDate = entry.DateCreated.ToPrettyDate(),
                IsPublished = entry.IsPublished ?? true
            };
        }
    }
}
