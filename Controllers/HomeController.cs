using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.IO;
using LandlordV3_MVC;

namespace LandlordV3_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginData loginData)
        {
            if (ModelState.IsValid)
            {
                using (LandlordV3Entities db = new LandlordV3Entities())
                {
                    var obj1 = db.LoginDatas.Where(a => a.Email.Equals(loginData.Email) && a.Password.Equals(loginData.Password)).FirstOrDefault();
                    if (obj1 != null)
                    {
                        Session["UserID"] = obj1.ID.ToString();

                        var obj2 = db.LoginRoles.Where(b => b.ID.Equals(obj1.RoleID)).FirstOrDefault();
                        if (obj2 != null)
                        {
                            string role = obj2.Type.ToString();
                            Session["Role"] = role;

                            var obj3 = db.PersonalDatas.Where(c => c.Email.Equals(obj1.Email)).FirstOrDefault();
                            if (obj3 != null)
                            {
                                Session["PersonalID"] = obj3.ID.ToString();
                                Session["FirstName"] = obj3.FirstName.ToString();
                                Session["LastName"] = obj3.LastName.ToString();
                            }
                            else
                            {
                                Session["FirstName"] = "Dear";
                                Session["LastName"] = role;
                            }

                            Guid personalID = obj3.ID; switch (role)
                            {
                                case "Tenant":
                                    return RedirectToAction("Index", "Tenants");
                                case "Manager":
                                    return RedirectToAction("Index", "Management");
                                case "Master":
                                    return RedirectToAction("Index", "Management");
                                default:
                                    return RedirectToAction("Login", "Home");
                            }
                        }// else (obj2 == null) == Bad Role ID...
                    }// else (obj1 == null) == Email / Password Combo...
                }
            }
            return View(loginData);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}