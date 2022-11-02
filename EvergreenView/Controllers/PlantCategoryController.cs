using EvergreenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace EvergreenView.Controllers
{
    public class PlantCategoryController : Controller
    {
        private string PlantCategoryApiUrl = "";
        private readonly HttpClient client = null;

        public PlantCategoryController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PlantCategoryApiUrl = "https://localhost:44334/api/PlantCategory";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(PlantCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<PlantCategory> plantCategories = JsonSerializer.Deserialize<List<PlantCategory>>(strData, options);
            return View(plantCategories);
        }

        public async Task<ActionResult> Details(int id)
        {
            var plant = await GetPlantCategoryById(id);
            if (plant == null)
                return NotFound();
            return View(plant);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlantCategory plantCategory)
        {
            string data = JsonSerializer.Serialize(plantCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(PlantCategoryApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(PlantCategoryApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            PlantCategory medicineCategory = JsonSerializer.Deserialize<PlantCategory>(strData, options);

            return View(medicineCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PlantCategory plantCategory)
        {
            string data = JsonSerializer.Serialize(plantCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(PlantCategoryApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<PlantCategory> GetPlantCategoryById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(PlantCategoryApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<PlantCategory>(strData, options);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var plant = await GetPlantCategoryById(id);
            if (plant == null)
                return NotFound();
            return View(plant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plant = await GetPlantCategoryById(id);
            HttpResponseMessage response = await client.DeleteAsync(PlantCategoryApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
