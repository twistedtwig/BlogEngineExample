using System.Web.Mvc;
using BlogEngine.Domain.Interfaces;

namespace WebApp.Controllers
{
    public class TagsController : Controller
    {
        private readonly IBlogEntryService _blogEntryService;

        public TagsController(IBlogEntryService blogEntryService)
        {
            _blogEntryService = blogEntryService;
        }

        public ActionResult Index(string tag)
        {
            ViewBag.Tag = tag;
            var blogEntries = _blogEntryService.GetList(new []{ tag });
            return View(blogEntries);
        }
    }
}