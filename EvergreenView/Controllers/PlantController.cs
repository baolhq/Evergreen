using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;

namespace EvergreenView.Controllers
{
    
    public class PlantController : Controller
    {
        private string PlantApiUrl = "";
        private string PlantCategoryApiUrl = "";
        private readonly HttpClient client = null;

        public PlantController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            PlantApiUrl = "https://localhost:44334/api/Plant";
            PlantCategoryApiUrl = "https://localhost:44334/api/PlantCategory";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(PlantApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Plant> plants = JsonSerializer.Deserialize<List<Plant>>(strData, options);
            return View(plants);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var plant = await GetPlantById(id);
            if (plant == null)
                return NotFound();
            return View(plant);
        }

        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(PlantCategoryApiUrl);
            string strData = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<PlantCategory> listPlantCategory = JsonSerializer.Deserialize<List<PlantCategory>>(strData, options);
            ViewData["PlantCategories"] = new SelectList(listPlantCategory, "PlantCategoryId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            string data = JsonSerializer.Serialize(plant);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(PlantApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(PlantCategoryApiUrl);
            string strData = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<PlantCategory> listPlantCategory = JsonSerializer.Deserialize<List<PlantCategory>>(strData, options);
            ViewData["PlantCategories"] = new SelectList(listPlantCategory, "PlantCategoryId", "Name");

            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await client.GetAsync(PlantApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Plant plant = JsonSerializer.Deserialize<Plant>(strData, options);

            HttpResponseMessage responeMedicineCategory = await client.GetAsync(PlantCategoryApiUrl);
            string strData1 = await responeMedicineCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<PlantCategory> listPlantCategory = JsonSerializer.Deserialize<List<PlantCategory>>(strData1, options1);
            ViewData["PlantCategories"] = new SelectList(listPlantCategory, "PlantCategoryId", "Name");

            return View(plant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Plant plant)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            string data = JsonSerializer.Serialize(plant);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(PlantApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage responePlantCategory = await client.GetAsync(PlantCategoryApiUrl);
            string strData1 = await responePlantCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<PlantCategory> listPlantCategory = JsonSerializer.Deserialize<List<PlantCategory>>(strData1, options1);
            ViewData["PlantCategories"] = new SelectList(listPlantCategory, "PlantCategoryId", "Name");

            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var plant = await GetPlantById(id);
            if (plant == null)
                return NotFound();
            await SetViewData();
            return View(plant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var plant = await GetPlantById(id);
            HttpResponseMessage response = await client.DeleteAsync(PlantApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<Plant> GetPlantById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(PlantApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Plant>(strData, options);
        }

        public async Task<IEnumerable<PlantCategory>> GetPlantCategories()
        {
            HttpResponseMessage response = await client.GetAsync(PlantCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<PlantCategory> listPlantCategory = JsonSerializer.Deserialize<List<PlantCategory>>(strData, options);
            return listPlantCategory;
        }

        public async Task SetViewData()
        {
            var listPlantCategory = await GetPlantCategories();
            ViewData["PlantCategories"] = new SelectList(listPlantCategory, "PlantCategoryId", "Name");
        }
    }
}
