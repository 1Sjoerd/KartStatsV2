using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KartStatsV2.BLL;
using KartStatsV2.Models;
using System.Configuration;
using KartStatsV2.DAL;

namespace KartStatsV2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _userService = new UserService(connectionString);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userService.Authenticate(model.Username, model.Password))
                {
                    // Gebruiker is succesvol geauthenticeerd, stel een sessievariabele in.
                    int id = _userService.GetIdByUsername(model.Username);
                    Session["Username"] = model.Username;
                    Session["Id"] = id;
                    return RedirectToAction("SecurePage", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Ongeldige gebruikersnaam of wachtwoord.");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email
                };

                _userService.RegisterUser(newUser, model.Password);

                // Gebruiker is succesvol geregistreerd, leid ze om naar de inlogpagina of een andere gewenste pagina.
                return RedirectToAction("Login");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            // Verwijder de sessievariabele om de gebruiker uit te loggen.
            Session.Remove("Id");

            // Leid de gebruiker om naar de inlogpagina.
            return RedirectToAction("Login");
        }


    }
}