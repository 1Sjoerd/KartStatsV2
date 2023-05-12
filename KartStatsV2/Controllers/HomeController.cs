using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KartStatsV2.Controllers
{
    public class HomeController : Controller
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

        public ActionResult SecurePage()
        {
            // Controleer of de gebruiker is ingelogd.
            if (Session["Id"] == null)
            {
                // Gebruiker is niet ingelogd, leid ze om naar de inlogpagina.
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}