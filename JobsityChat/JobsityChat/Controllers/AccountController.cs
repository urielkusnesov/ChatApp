using Model;
using Service.UserService;
using System.Web.Mvc;
using System.Web.Security;

namespace JobsityChat.Controllers
{
    public class AccountController : Controller
    {
        private IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.Username, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Chat", "Home", null);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Invalid user or password");
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User model, string confirm)
        {
            if (ModelState.IsValid)
            {
                if(model.Password != confirm)
                {
                    ModelState.AddModelError("", "Passwords do not match");
                }
                else
                {
                    try
                    {
                        userService.Add(model.Username, model.Password);
                        return RedirectToAction("Index", "Home");
                    }
                    catch (MembershipCreateUserException e)
                    {
                        ModelState.AddModelError("", "Cannot create user. Please try again");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid registration");
            return View(model);
        }
    }
}