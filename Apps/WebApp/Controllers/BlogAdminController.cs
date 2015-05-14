using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogEngine.Domain.Extensions;
using BlogEngine.Domain.Interfaces;
using BlogEngine.Domain.Models;
using WebApp.Models.Blog;

namespace WebApp.Controllers
{
    [Authorize]
    public class BlogAdminController : Controller
    {
        private readonly IBlogEntryService _blogService;

        public BlogAdminController(IBlogEntryService blogService)
        {
            _blogService = blogService;
        }

        public ActionResult Index()
        {
            return View(_blogService.GetList(false));
        }

        [HttpGet]
        public ActionResult Show([Bind(Prefix = "id")] string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentNullException("slug");
            }

            BlogEntryModel entry;
            try
            {
                entry = _blogService.GetBySlug(slug);
            }
            catch (Exception ex)
            {
                throw new HttpException(404, "Entry not found", ex);
            }

            return View(entry);
        }


        [HttpGet]
        public ActionResult Add()
        {
            return View("Edit", new BlogEntryModel());
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Add(BlogEntryModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model);

            var serviceResult = _blogService.Save(model);
            if (serviceResult.Succeeded)
            {
                return RedirectToAction("Show", "BlogAdmin", new { id = model.Title.ToUrlSlug() });
            }

            ModelState.AddModelError("Title", serviceResult.Errors.Any() ? serviceResult.Errors[0] : "There was an error");
            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult Edit([Bind(Prefix = "id")] string slug)
        {
            return View(_blogService.GetBySlug(slug));
        }

        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(BlogEntryModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var serviceResult = _blogService.Save(model);
            if (serviceResult.Succeeded)
            {
                return RedirectToAction("Index", "BlogAdmin");
            }

            ModelState.AddModelError("Title", serviceResult.Errors.Any() ? serviceResult.Errors[0] : "There was an error");
            return View(model);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            _blogService.Delete(model.Slug);
            return RedirectToAction("Index", "BlogAdmin");
        }

        [HttpGet]
        public ActionResult Delete([Bind(Prefix = "id")] string slug)
        {
            var entry = _blogService.GetBySlug(slug);

            var model = new DeleteModel
            {
                Title = entry.Title,
                Slug = slug
            };

            return View(model);
        }
    }
}