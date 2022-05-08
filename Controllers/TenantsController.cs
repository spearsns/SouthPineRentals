using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LandlordAppMVC;
using LandlordV3_MVC;

namespace LandlordAppMVC.Controllers
{
    public class TenantsController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            InitAppController(filterContext);
            base.OnActionExecuting(filterContext);
        }

        // Redirect to login page if user's session is not valid.
        public void InitAppController(ActionExecutingContext filterContext)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "Tenant")
            {
                filterContext.Result = RedirectToAction("Login", "Home");
            }
        }

        private LandlordV3Entities db = new LandlordV3Entities();

        public ActionResult Index()
        {
            return View();
        }
        /* --V2 Version
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Requests()
        {
            return View();
        }
        public ActionResult Help()
        {
            return View();
        }
        */
    }
}
