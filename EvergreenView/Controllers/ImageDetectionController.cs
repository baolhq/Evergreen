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

            DetectionApiUrl = "https://localhost:44334/api/DetectionHistory";
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

            string query = "/" + session.GetString("i");
            HttpResponseMessage response = await client.GetAsync(DetectionApiUrl + query);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<ExtractDetectionHistoriesDTO> history =
                JsonSerializer.Deserialize<List<ExtractDetectionHistoriesDTO>>(strData, options);

            var result = JsonSerializer.Serialize(history);
            ViewBag.history = result;

            return View();
        }
    }
}
