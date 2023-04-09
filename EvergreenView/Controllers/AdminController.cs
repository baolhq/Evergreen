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
            _medicineCategoryApiUrl = "https://evergreen-api.onrender.com/api/MedicineCategory/GetMedicineCategoryName";
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("t");
            if (HttpContext.Session.GetString("r") != "Admin" || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.GetAsync(_medicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var medicineCategoriesName = JsonSerializer.Deserialize<JsonElement>(strData, options);
            var listCategoriesName = medicineCategoriesName.GetProperty("listCategoriesName");
            HttpContext.Session.SetString("medicineCategoriesName", listCategoriesName.ToString());
            return View();
        }
    }
}
