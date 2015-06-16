using BlogEngine.Domain.Models;
using BlogEngine.Repository.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UserManagementService.Interfaces;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService<BlogUser, BlogUserEntity, int> _userService;

        public AccountController(IUserService<BlogUser, BlogUserEntity, int> userService)
        {
            _userService = userService;

            //TODO remove this after first run
            //this is here just to show how to boost strap your first user when the project is first setup.
            //_userService.Create(new BlogUser {Email = "MYEMAILHERE", DisplayName = "MY DISPLAY NAME HERE", UserName = "MYUSERNAMEHERE"}, "MYPASSWORDHERE");
        }

        


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.isLogin = true;

            RegisterReturnUrl(returnUrl);

            return this.View();
        }

        private void RegisterReturnUrl(string returnUrl)
        {
            //So that the user can be referred back to where they were when they click logon
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
            {
                ViewBag.ReturnURL = returnUrl;
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            RegisterReturnUrl(returnUrl);

            ViewBag.isLogin = true;
            string decodedUrl = "";
            if (!string.IsNullOrEmpty(returnUrl))
            {
                decodedUrl = Server.UrlDecode(returnUrl);
            }

            if (ModelState.IsValid)
            {
                var user = _userService.Find(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    if (Url.IsLocalUrl(decodedUrl))
                    {
                        return Redirect(decodedUrl);
                    }
                    return RedirectToAction("Index", "Home");
                    
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }

//        //
//        // POST: /Account/Register
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Register(RegisterViewModel model)
//        {
//            if (this.ModelState.IsValid)
//            {
//                var user = new ApplicationUser { UserName = model.Email, Person = new PersonDetails { DisplayName = model.Title + " " + model.FirstName + " " + model.LastName } };
//                var result = await this.CommonControllerLogic.IdentityService.CreateAsync(user, model.Password);
//                if (result.Succeeded)
//                {
//                    await this.SignInAsync(user, isPersistent: false);
//                    return this.RedirectToAction("Index", "Home");
//                }
//                this.AddErrors(result);
//            }
//
//            // If we got this far, something failed, redisplay form
//            return this.View(model);
//        }
//
//        //
//        // POST: /Account/Disassociate
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
//        {
//            IdentityResult result = await this.CommonControllerLogic.IdentityService.RemoveLoginAsync(this.User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
//            ManageMessageId? message = result.Succeeded ? ManageMessageId.RemoveLoginSuccess : ManageMessageId.Error;
//            return this.RedirectToAction("Manage", new { Message = message });
//        }
//
//        //
//        // GET: /Account/Manage
//        public ActionResult Manage(ManageMessageId? message)
//        {
//            this.ViewBag.StatusMessage =
//                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
//                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
//                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
//                : message == ManageMessageId.Error ? "An error has occurred."
//                : "";
//            this.ViewBag.HasLocalPassword = this.HasPassword();
//            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
//            return this.View();
//        }
//
//        //
//        // POST: /Account/Manage
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> Manage(ManageUserViewModel model)
//        {
//            bool hasPassword = this.HasPassword();
//            this.ViewBag.HasLocalPassword = hasPassword;
//            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
//            if (hasPassword)
//            {
//                if (this.ModelState.IsValid)
//                {
//                    IdentityResult result = await this.CommonControllerLogic.IdentityService.ChangePasswordAsync(this.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
//                    if (result.Succeeded)
//                    {
//                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
//                    }
//                    this.AddErrors(result);
//                }
//            }
//            else
//            {
//                // User does not have a password so remove any validation errors caused by a missing OldPassword field
//                var state = this.ModelState["OldPassword"];
//                if (state != null)
//                {
//                    state.Errors.Clear();
//                }
//
//                if (this.ModelState.IsValid)
//                {
//                    IdentityResult result = await this.CommonControllerLogic.IdentityService.AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);
//                    if (result.Succeeded)
//                    {
//                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
//                    }
//                    this.AddErrors(result);
//                }
//            }
//
//            // If we got this far, something failed, redisplay form
//            return this.View(model);
//        }
//
//        //
//        // POST: /Account/ExternalLogin
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public ActionResult ExternalLogin(string provider, string returnUrl)
//        {
//            // Request a redirect to the external login provider
//            return new ChallengeResult(provider, this.Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
//        }
//
//        //
//        // GET: /Account/ExternalLoginCallback
//        [AllowAnonymous]
//        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
//        {
//            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
//            if (loginInfo == null)
//            {
//                return this.RedirectToAction("Login");
//            }
//
//            // Sign in the user with this external login provider if the user already has a login
//            var user = await this.CommonControllerLogic.IdentityService.FindAsync(loginInfo.Login);
//            if (user != null)
//            {
//                await this.SignInAsync(user, isPersistent: false);
//                return this.RedirectToLocal(returnUrl);
//            }
//
//            // If the user does not have an account, then prompt the user to create an account
//            this.ViewBag.ReturnUrl = returnUrl;
//            this.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
//            return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
//        }
//
//        //
//        // POST: /Account/LinkLogin
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult LinkLogin(string provider)
//        {
//            // Request a redirect to the external login provider to link a login for the current user
//            return new ChallengeResult(provider, this.Url.Action("LinkLoginCallback", "Account"), this.User.Identity.GetUserId());
//        }
//
//        //
//        // GET: /Account/LinkLoginCallback
//        public async Task<ActionResult> LinkLoginCallback()
//        {
//            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, this.User.Identity.GetUserId());
//            if (loginInfo == null)
//            {
//                return this.RedirectToAction("Manage", new { Message = ManageMessageId.Error });
//            }
//            var result = await this.CommonControllerLogic.IdentityService.AddLoginAsync(this.User.Identity.GetUserId(), loginInfo.Login);
//            if (result.Succeeded)
//            {
//                return this.RedirectToAction("Manage");
//            }
//            return this.RedirectToAction("Manage", new { Message = ManageMessageId.Error });
//        }
//
//        //
//        // POST: /Account/ExternalLoginConfirmation
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
//        {
//            if (this.User.Identity.IsAuthenticated)
//            {
//                return this.RedirectToAction("Manage");
//            }
//
//            if (this.ModelState.IsValid)
//            {
//                // Get the information about the user from the external login provider
//                var info = await this.AuthenticationManager.GetExternalLoginInfoAsync();
//                if (info == null)
//                {
//                    return this.View("ExternalLoginFailure");
//                }
//                var user = new ApplicationUser { UserName = model.UserName };
//                var result = await this.CommonControllerLogic.IdentityService.CreateAsync(user);
//                if (result.Succeeded)
//                {
//                    result = await this.CommonControllerLogic.IdentityService.AddLoginAsync(user.Id, info.Login);
//                    if (result.Succeeded)
//                    {
//                        await this.SignInAsync(user, isPersistent: false);
//                        return this.RedirectToLocal(returnUrl);
//                    }
//                }
//                this.AddErrors(result);
//            }
//
//            this.ViewBag.ReturnUrl = returnUrl;
//            return this.View(model);
//        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LogOffLink
        [HttpGet]
        public ActionResult LogOffLink()
        {
            return LogOff();
        }

//        //
//        // GET: /Account/ExternalLoginFailure
//        [AllowAnonymous]
//        public ActionResult ExternalLoginFailure()
//        {
//            return this.View();
//        }

        // TODO: Tidy this up
        //        [ChildActionOnly]
        //        public ActionResult RemoveAccountList()
        //        {
        //            var linkedAccounts = this.CommonControllerLogic.UserService.GetLogins(this.User.Identity.GetUserId());
        //            this.ViewBag.ShowRemoveButton = this.HasPassword() || linkedAccounts.Count > 1;
        //            return (ActionResult)this.PartialView("_RemoveAccountPartial", linkedAccounts);
        //        }

        //        protected override void Dispose(bool disposing)
        //        {
        //            if (disposing && this.CommonControllerLogic.UserService != null)
        //            {
        //                this.CommonControllerLogic.UserService.Dispose();
        //            }
        //            base.Dispose(disposing);
        //        }

//        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(BlogUser user, bool isPersistent)
        {
//            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = _userService.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            this.AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError("", error);
            }
        }
//
//        private bool HasPassword()
//        {
//            var user = this.CommonControllerLogic.IdentityService.FindById(this.User.Identity.GetUserId());
//            if (user != null)
//            {
//                return user.HasLocalPassword;
//            }
//            return false;
//        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

//        private ActionResult RedirectToLocal(string returnUrl)
//        {
//            if (this.Url.IsLocalUrl(returnUrl))
//            {
//                return this.Redirect(returnUrl);
//            }
//            return this.RedirectToAction("Index", "Home");
//        }
//
//        private class ChallengeResult : HttpUnauthorizedResult
//        {
//            public ChallengeResult(string provider, string redirectUri)
//                : this(provider, redirectUri, null)
//            {
//            }
//
//            public ChallengeResult(string provider, string redirectUri, string userId)
//            {
//                this.LoginProvider = provider;
//                this.RedirectUri = redirectUri;
//                this.UserId = userId;
//            }
//
//            public string LoginProvider { get; set; }
//            public string RedirectUri { get; set; }
//            public string UserId { get; set; }
//
//            public override void ExecuteResult(ControllerContext context)
//            {
//                var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
//                if (this.UserId != null)
//                {
//                    properties.Dictionary[XsrfKey] = this.UserId;
//                }
//                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
//            }
//        }
//        #endregion
    }
}