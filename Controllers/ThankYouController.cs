using Microsoft.AspNetCore.Mvc;

namespace PL_APP_MVC.Controllers
{
    public class ThankYouController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
