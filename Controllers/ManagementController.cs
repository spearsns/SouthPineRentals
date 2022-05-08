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
    public class ManagementController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            InitAppController(filterContext);
            base.OnActionExecuting(filterContext);
        }

        // Redirect to login page if user's session is not valid.
        public void InitAppController(ActionExecutingContext filterContext)
        {
            if (Session["Role"] == null) filterContext.Result = RedirectToAction("Login", "Home");
            else if (Session["Role"].ToString() != "Manager" && Session["Role"].ToString() != "Owner")
            {
                filterContext.Result = RedirectToAction("Login", "Home");
            }
        }

        private LandlordV3Entities db = new LandlordV3Entities();

        // GET: Management
        public ActionResult Index()
        {
            return View();
        }
        /* --V2 Version
        public ActionResult Properties()
        {
            return View();
        }
        public ActionResult Maintenance()
        {
            return View();
        }
        public ActionResult Applications()
        {
            return View();
        }
        public ActionResult Tenants()
        {
            return View();
        }
        public ActionResult Requests()
        {
            return View();
        }
        public ActionResult Messages()
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