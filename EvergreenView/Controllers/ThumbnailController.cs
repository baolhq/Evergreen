using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvergreenView.Controllers
{
    public class ThumbnailController : Controller
    {
        private readonly HttpClient client = null;
        private string ThumnailApiUrl = "";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ThumbnailController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            ThumnailApiUrl = "https://localhost:44334/api/Thumbnail";
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
            if (HttpContext.Session.GetString("r") != "Admin")
                return RedirectToAction("Index", "Home");

            var response = await client.GetAsync(ThumnailApiUrl);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData, options);
            return View(listImages);
        }
    }
}
