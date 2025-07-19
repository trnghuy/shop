using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
