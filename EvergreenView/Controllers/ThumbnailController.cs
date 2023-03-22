using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.IO;

namespace EvergreenView.Controllers
{
    public class ThumbnailController : Controller
    {
        private readonly HttpClient client = null;
        private string ThumbnailApiUrl = "";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ThumbnailController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            ThumbnailApiUrl = "https://evergreen-api.onrender.com/api/Thumbnail";
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

            var response = await client.GetAsync(ThumbnailApiUrl);
            var strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData, options);
            return View(listImages);
        }

        public IActionResult Create()
        {
            ViewData["t"] = HttpContext.Session.GetString("t");
            return View();
        }

        public async Task<ActionResult> Update(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
                return RedirectToAction("Index");

            var thumbnail = await GetThumbnailAsync(id);
            if (thumbnail == null) return NotFound();

            ViewData["t"] = HttpContext.Session.GetString("t");
            return View(thumbnail);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
                return RedirectToAction("Index");

            var thumbnail = await GetThumbnailAsync(id);
            if (thumbnail == null) return NotFound();
            return View(thumbnail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Thumbnail thumbnail)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
                return RedirectToAction("Index");

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (thumbnail == null) return NotFound();

            var data = JsonSerializer.Serialize(thumbnail);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.DeleteAsync(ThumbnailApiUrl + "/" + thumbnail.ThumbnailId);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(thumbnail);
        }

        private async Task<Thumbnail> GetThumbnailAsync(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
                return null;

            HttpResponseMessage response = await client.GetAsync(ThumbnailApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var thumbnail = JsonSerializer.Deserialize<Thumbnail>(strData, options);
            return thumbnail;
        }
    }
}
