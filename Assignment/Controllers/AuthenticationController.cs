using Assignment.Data;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext db;

        public AuthenticationController(ApplicationDbContext _db)
        {
            db= _db;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User userModel)
        {
            if (db.Users.Any(c=>c.EmailAdress==userModel.EmailAdress))
            {
                var user = db.Users.FirstOrDefault(c=>c.EmailAdress.Equals(userModel.EmailAdress));
                if (user != null)
                {
                    if (user.Password==userModel.Password)
                    {
                        //TempData["success"] = "LoggedIn Successfully";
                        HttpContext.Session.SetString("UserEmail", userModel.EmailAdress);
                        return RedirectToAction("Index", "Products");
                    }
                    TempData["error"] = "Incorrect Password";
                    return View();
                }
                
            }
            TempData["error"] = "User not found";
            return View();
        }

        [HttpPost]
        public IActionResult Register(User userModel)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(c => c.EmailAdress == userModel.EmailAdress))
                {
                    TempData["error"] = "Email already exists";
                    return View();
                }
                db.Users.Add(userModel);
                db.SaveChanges();
                TempData["success"] = "Registered successfully";
                return RedirectToAction("Login", "Authentication");
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
