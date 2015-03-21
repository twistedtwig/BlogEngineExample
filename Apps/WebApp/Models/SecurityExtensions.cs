using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public static class SecurityExtensions
    {
        /// <summary>
        /// Sanitises HTML fragment for protection against XSS vulnerabilities
        /// </summary>
        public static IHtmlString Safe(this HtmlHelper html, string unsafeHtml)
        {
            /// removes <pre> - Tags and disables therefore prettify.js
            //var safeHtml = Sanitizer.GetSafeHtmlFragment(unsafeHtml);
            return MvcHtmlString.Create(unsafeHtml);
        }
    }
}