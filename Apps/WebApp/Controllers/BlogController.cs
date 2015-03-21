using System;
using System.Web;
using System.Web.Mvc;
using BlogEngine.Domain.Interfaces;
using BlogEngine.Domain.Models;

namespace WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogEntryService _blogService;

        public BlogController(IBlogEntryService blogService)
        {
            _blogService = blogService;
        }

        // GET: Blog
        public ActionResult Index()
        {
            return View(_blogService.GetList());
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
    }
}