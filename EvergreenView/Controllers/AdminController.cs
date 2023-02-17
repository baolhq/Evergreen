using Microsoft.AspNetCore.Mvc;

namespace EvergreenView.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
