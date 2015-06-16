using AutoMapper;
using BlogEngine.Domain.Models;
using BlogEngine.Repository.Models;

namespace WebApp
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new UserProfile());
                cfg.AddProfile(new BlogProfile());
            });

            Mapper.AssertConfigurationIsValid();
        }
    }

    public class UserProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BlogUser, BlogUserEntity>().ReverseMap();
        }
    }

    public class BlogProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BlogEntry, BlogEntryEntity>().ReverseMap();
            Mapper.CreateMap<TagEntity, Tag>().ReverseMap();
        }
    }


}