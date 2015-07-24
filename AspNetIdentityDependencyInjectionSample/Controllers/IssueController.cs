using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetIdentityDependencyInjectionSample.DataLayer.Context;

namespace AspNetIdentityDependencyInjectionSample.Controllers
{
    public class IssueController : Controller
    {
        private IUnitOfWork _uow;

        public IssueController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ActionResult IssueWidget()
        {
            return Content("Here's where issues would go!");
        }
    }
}