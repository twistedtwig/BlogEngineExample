using System.Web.Mvc;
using BlogEngine.Domain.Interfaces;

namespace WebApp.Controllers
{
    public class BlogCountController : Controller
    {
        private readonly IBlogTagService _blogTagService;

        public BlogCountController(IBlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        // GET: BlogCount
        public ActionResult Index()
        {
            return View(_blogTagService.GetCount());
        }
    }
}