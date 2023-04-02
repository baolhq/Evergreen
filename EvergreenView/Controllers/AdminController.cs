using EvergreenAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvergreenView.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _medicineCategoryApiUrl;
        private readonly HttpClient _client;

        public AdminController()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _medicineCategoryApiUrl = "https://evergreen-api.onrender.com/api/MedicineCategory";
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            HttpResponseMessage response = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var medicineCategoriesName = JsonSerializer.Deserialize<JsonElement>(strData, options);
            HttpContext.Session.SetString("medicineCategoriesName", medicineCategoriesName.ToString());
            return View();
        }
    }
}
