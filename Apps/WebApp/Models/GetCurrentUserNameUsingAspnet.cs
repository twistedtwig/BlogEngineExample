using System.Web;
using BlogEngine.Domain.Interfaces;

namespace WebApp.Models
{
    public class GetCurrentUserNameUsingAspnet : IGetCurrentUserName
    {
        public string GetUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }
}