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
    public class ImageController : Controller
    {
        private readonly HttpClient client = null;
        private string ImageApiUrl = "";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ImageController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            ImageApiUrl = "https://localhost:44334/api/Image";
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
            {
                return RedirectToAction("Index", "Home");
            }

            string query = "";
            HttpResponseMessage response = await client.GetAsync(ImageApiUrl + query);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Image> listImages = JsonSerializer.Deserialize<List<Image>>(strData, options);
            return View(listImages);
        }
    }
}
