using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : baseController
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult Test()
        {
            return View();
        }

        //建立PartilaView, 從About頁面顯示出來
        public ActionResult PartialAbout()
        {
            ViewBag.Message = "Your application description page.";

            if (Request.IsAjaxRequest())
            {
                return PartialView("About");
            }
            else
            {
                return View("About");
            }
        }

        public ActionResult SomeAction()
        {
            return PartialView("SuccessRedirect", "/");
        }

        public ActionResult GetFile()
        {
            //return File(Server.MapPath("~/Content/fileList.jpg"), "image/jpeg", "NewName_fileList.jpg");
            return File(Server.MapPath("~/Content/fileList.jpg"), "image/jpeg");

        }

        public ActionResult GetJson()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return Json(db.Product.Take(5),JsonRequestBehavior.AllowGet);
        }
    }
}