using System.Web.Mvc;

namespace AspNetIdentityDependencyInjectionSample.Controllers
{
    [Authorize]
    public class SignalRTestController : Controller
    {
        // GET: SignalRTest
        public ActionResult Index()
        {
            return View();
        }
    }
}