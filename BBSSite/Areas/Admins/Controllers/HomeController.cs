using BBSSite.MyPublic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BBSSite.Areas.Admins.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admins/Home
        [MyAuthorize(0)]
        public ActionResult Index()
        {
            PublicFunctions.SetUrls(ViewBag, Url);
            return View();
        }
    }
}