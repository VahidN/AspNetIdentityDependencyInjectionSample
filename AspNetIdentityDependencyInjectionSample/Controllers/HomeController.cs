using System.Web.Mvc;
using AspNetIdentityDependencyInjectionSample.DataLayer.Context;

namespace AspNetIdentityDependencyInjectionSample.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _uow;

        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
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
    }
}
