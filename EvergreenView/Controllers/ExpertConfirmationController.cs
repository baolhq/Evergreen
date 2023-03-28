using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EvergreenView.Controllers
{
    public class ExpertConfirmationController : Controller
    {
        private readonly string _expertConfirmationApi;
        private readonly string _thumbnailApiUrl;
        private readonly string _detectionHistoryApiUrl;

        public ExpertConfirmationController()
        {
            _expertConfirmationApi = "https://evergreen-api.onrender.com/api/ExpertConfirmation";
            _thumbnailApiUrl = "https://evergreen-api.onrender.com/api/Thumbnail";
            _detectionHistoryApiUrl = "https://evergreen-api.onrender.com/api/DetectionHistory";
        }
        
        // GET
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("r") != "Admin" || HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }
    }
}