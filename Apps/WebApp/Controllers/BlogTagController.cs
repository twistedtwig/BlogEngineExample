using System.Web.Http;
using BlogEngine.Domain.Interfaces;

namespace WebApp.Controllers
{
    public class BlogTagController : ApiController
    {
        private readonly IBlogTagService _blogTagService;

        public BlogTagController(IBlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        public IHttpActionResult GetTags(string text)
        {
            return Ok(_blogTagService.GetPossibles(text));
        }
    }
}
