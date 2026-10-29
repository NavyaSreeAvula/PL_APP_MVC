using Microsoft.AspNetCore.Mvc;
using PL_APP_MVC.Models;

namespace PL_APP_MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login() {

            return View();
        }

        public IActionResult LoginCheck(string username, string password) {
            if (username == "admin" && password == "admin")
            {
                return RedirectToAction("Index", "Manager");
            }
            else {
                ViewBag.message = "Invalid Credentials";
                return View("Login");
            }
        }
    }
}
    