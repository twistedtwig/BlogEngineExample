using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BlogEngine.Domain.Implementations;
using BlogEngine.Domain.Interfaces;
using BlogEngine.Domain.Models;
using BlogEngine.Repository.Interfaces;
using BlogEngine.Repository.Models;
using Moq;
using NUnit.Framework;
using Testing;

namespace BlogEngine.Domain.LogicTests
{
    [TestFixture]
    public class BlogEntryServiceTests
    {
        [Test]
        public void TestAddNewTagWillSaveTagEntity()
        {
            const string slug = "this-is-a-slug";

            var mocker = new AutoMock<BlogEntryService>();

//            mocker.GetMock<IBlogUserService>().Setup(x => x.Current).Returns(new BlogUser { FriendlyName = "Jon Hawkins" });

            mocker.GetMock<IBlogRepository>().Setup(x => x.All<TagEntity>()).Returns(new List<TagEntity> { new TagEntity { Id = 1, Name = "c#" }, new TagEntity { Id = 3, Name = "php" }, new TagEntity { Id = 5, Name = "javascript" } });
            mocker.GetMock<IBlogRepository>().Setup(x => x.Exists(It.IsAny<Expression<Func<BlogEntryEntity, bool>>>())).Returns(true);
            mocker.GetMock<IBlogRepository>().Setup(x => x.Single(It.IsAny<Expression<Func<BlogEntryEntity, bool>>>())).Returns(new BlogEntryEntity { Slug = slug });


            var blogEntryModel = new BlogEntryModel();

            blogEntryModel.Slug = slug;
            blogEntryModel.Html = "some html";
            blogEntryModel.Date = DateTime.Now.ToString("dd/MM/yyyy");
            blogEntryModel.TagsString = "c# happy";

           mocker.Object.Save(blogEntryModel);           

            mocker.GetMock<IBlogRepository>().Verify(x => x.Add(It.Is<TagEntity>(t => t.Name == "happy")));
        }

        [Test]
        public void TestSavingWillRecountTags()
        {
            const string slug = "this-is-a-slug";

            var mocker = new AutoMock<BlogEntryService>();

//            mocker.GetMock<IBlogUserService>().Setup(x => x.Current).Returns(new BlogUser { FriendlyName = "Jon Hawkins" });

            mocker.GetMock<IBlogRepository>().Setup(x => x.All<TagEntity>()).Returns(new List<TagEntity> { new TagEntity { Id = 1, Name = "c#" }, new TagEntity { Id = 3, Name = "php" }, new TagEntity { Id = 5, Name = "javascript" } });
            mocker.GetMock<IBlogRepository>().Setup(x => x.Exists(It.IsAny<Expression<Func<BlogEntryEntity, bool>>>())).Returns(true);
            mocker.GetMock<IBlogRepository>().Setup(x => x.Single(It.IsAny<Expression<Func<BlogEntryEntity, bool>>>())).Returns(new BlogEntryEntity { Slug = slug });


            var blogEntryModel = new BlogEntryModel();

            blogEntryModel.Slug = slug;
            blogEntryModel.Html = "some html";
            blogEntryModel.Date = DateTime.Now.ToString("dd/MM/yyyy");
            blogEntryModel.TagsString = "c# happy";

            var serviceResult = mocker.Object.Save(blogEntryModel);
            Assert.IsNotNull(serviceResult);
            Assert.IsTrue(serviceResult.Succeeded);

            mocker.GetMock<IBlogTagService>().Verify(x => x.UpdateTagCount());
            mocker.GetMock<IBlogTagService>().Verify(x => x.RemoveUnsedTags());
        }


        [Test]
        public void TestSaveAddsCorrectTags()
        {
            const string slug = "this-is-a-slug";

            var mocker = new AutoMock<BlogEntryService>();

//            mocker.GetMock<IBlogUserService>().Setup(x => x.Current).Returns(new BlogUser { FriendlyName = "Jon Hawkins" });
            
            mocker.GetMock<IBlogRepository>().Setup(x => x.All<TagEntity>()).Returns(new List<TagEntity> { new TagEntity { Id = 1, Name = "c#" }, new TagEntity { Id = 3, Name = "php" }, new TagEntity { Id = 5, Name = "javascript" } });
            mocker.GetMock<IBlogRepository>().Setup(x => x.Exists(It.IsAny<Expression<Func<BlogEntryEntity, bool>>>())).Returns(true);
            mocker.GetMock<IBlogRepository>().Setup(x => x.Single(It.IsAny<Expression<Func<BlogEntryEntity, bool>>>())).Returns(new BlogEntryEntity { Slug = slug});
            

            var blogEntryModel = new BlogEntryModel();
            
            blogEntryModel.Slug = slug;
            blogEntryModel.Html = "some html";
            blogEntryModel.Date = DateTime.Now.ToString("dd/MM/yyyy");
            blogEntryModel.TagsString = "c# happy";

            var serviceResult = mocker.Object.Save(blogEntryModel);
            Assert.IsNotNull(serviceResult);

            mocker.GetMock<IBlogRepository>().Verify(x => x.Save(It.Is<BlogEntryEntity>(b => 
                b.Slug == blogEntryModel.Slug
                && b.Tags.Count == 2
                && b.Tags.Any(t => t.Name == "c#" && t.Id == 1)
                && b.Tags.Any(t => t.Name == "happy" && t.Id == 0)
            ), It.IsAny<Expression<Func<BlogEntryEntity, bool>>>()));
        }       
    }
}
