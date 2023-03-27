using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvergreenView.Controllers
{
    public class ImageDetectionController : Controller
    {
        private readonly HttpClient client = null;
        private string DetectionApiUrl = "";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ImageDetectionController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            DetectionApiUrl = "https://evergreen-api.onrender.com/api/DetectionHistory";
            _config = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public ISession session
        {
            get
            {
                return _httpContextAccessor.HttpContext.Session;
            }
        }

        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("r")))
                return RedirectToAction("Login", "Authentication");

            var query = "/" + session.GetString("i");
            var response = await client.GetAsync(DetectionApiUrl + query);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var history = JsonSerializer.Deserialize<List<ExtractDetectionHistoriesDTO>>(strData, options);
            var result = JsonSerializer.Serialize(history);
            ViewBag.history = result;

            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("r")))
            //    return RedirectToAction("Login", "Authentication");

            var query = "/Details/" + id;
            var response = await client.GetAsync(DetectionApiUrl + query);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var accuracies = JsonSerializer.Deserialize<List<DetectionAccuracy>>(strData, options);
            return View(accuracies);
        }
    }
}
