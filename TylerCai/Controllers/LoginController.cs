using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using System.Data.Sql;
using TylerCai.ViewModels;
using TylerCai.DatabaseConnections;

namespace TylerCai.Controllers
{
    public class LoginController : Controller
    {
        DBContext db = new DBContext();

        public IActionResult Login()
        {
            return View(new UserViewModel());
        }

        public IActionResult Register()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public IActionResult CreateUser(UserViewModel user)
        {
            db.Connect();
            if (db.CreateUser(user))
            {
                db.Close();
                return View("Login");
            }
            ModelState.AddModelError("User", "User already exists");
            db.Close();
            return View("Register", user);
        }

        [HttpPost]
        public IActionResult Verify(UserViewModel user)
        {
            db.Connect();
            if (db.VerifyUser(user))
            {
                db.Close();
                return View("Contact", new UserViewModel { Email = user.Email });
            }
            else
            {
                db.Close();
                ModelState.AddModelError("User", "Username-password combination does not exist");
                return View("Login");
            }
        }
    }
}
