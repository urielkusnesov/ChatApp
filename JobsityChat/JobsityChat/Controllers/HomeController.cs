using Model;
using Service.PostService;
using System;
using System.Web.Mvc;

namespace JobsityChat.Controllers
{
    public class HomeController : Controller
    {
        private IPostService postService;

        public HomeController(IPostService postService)
        {
            this.postService = postService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Chat()
        {
            var posts = postService.List();
            ViewBag.Username = User.Identity.Name;
            return View(posts);
        }

        [Authorize]
        public void HandlePost(string name, string message)
        {
            if (message.StartsWith("/stock="))
            {
                Bot.GetQuote(message.Split('=')[1]);
            }
            else
            {
                postService.Add(new Post { User = name, Message = message, Date = DateTime.Now });
            }
        }
    }
}