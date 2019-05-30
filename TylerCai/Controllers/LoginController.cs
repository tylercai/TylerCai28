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
        DBContext db;

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
            db = new DBContext();
            db.Connect();
            bool created = db.CreateUser(user);
            return View("About");
            if (created)
            {
                db.Close();
                return View("Login", new UserViewModel());
            }
            ModelState.AddModelError("User", "User already exists");
            db.Close();
            return View("Register", user);
        }

        [HttpPost]
        public IActionResult Verify(UserViewModel user)
        {
            db = new DBContext();
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
                return View("Login", user);
            }
        }
    }
}
