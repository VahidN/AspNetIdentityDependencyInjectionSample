using System.Web.Mvc;
using AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts;

namespace AspNetIdentityDependencyInjectionSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        public HomeController(IApplicationUserManager userManager)
        {
            _userManager = userManager;
        }


        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetData()
        {
            return Content(_userManager.GetCurrentUser().UserName);
        }
    }
}
