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
    
    public class TreatmentController : Controller
    {
        private string TreatmentApiUrl = "";
        private string DiseaseApiUrl = "";
        private readonly HttpClient client = null;

        public TreatmentController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            TreatmentApiUrl = "https://localhost:44334/api/Treatment";
            DiseaseApiUrl = "https://localhost:44334/api/Disease";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(TreatmentApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Treatment> treatments = JsonSerializer.Deserialize<List<Treatment>>(strData, options);
            return View(treatments);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var treatment = await GetTreatmentById(id);
            if (treatment == null)
                return NotFound();
            return View(treatment);
        }

        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData = await responeDisease.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Treatment treatment)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(treatment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(TreatmentApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData = await responeDisease.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");

            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await client.GetAsync(TreatmentApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Treatment treatment = JsonSerializer.Deserialize<Treatment>(strData, options);

            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData1 = await responeDisease.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData1, options1);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");

            return View(treatment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Treatment treatment)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(treatment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(TreatmentApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData = await responeDisease.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");

            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var treatment = await GetTreatmentById(id);
            if (treatment == null)
                return NotFound();
            await SetViewData();
            return View(treatment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var plant = await GetTreatmentById(id);
            HttpResponseMessage response = await client.DeleteAsync(TreatmentApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<Treatment> GetTreatmentById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(TreatmentApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Treatment>(strData, options);
        }

        public async Task<IEnumerable<Disease>> GetPlantCategories()
        {
            HttpResponseMessage response = await client.GetAsync(DiseaseApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);
            return listDiseases;
        }

        public async Task SetViewData()
        {
            var listDiseases = await GetPlantCategories();
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");
        }

        public async Task<IActionResult> AdminIndex()
        {
            HttpResponseMessage response = await client.GetAsync(TreatmentApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Treatment> treatments = JsonSerializer.Deserialize<List<Treatment>>(strData, options);
            return View(treatments);
        }
    }
}
