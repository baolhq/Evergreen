using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EvergreenView.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
