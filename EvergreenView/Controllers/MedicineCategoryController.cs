﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;

namespace EvergreenView.Controllers
{
    public class MedicineCategoryController : Controller
    {
        private string MedicineCategoryApiUrl = "";
        private readonly HttpClient client = null;

        public MedicineCategoryController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MedicineCategoryApiUrl = "https://localhost:44334/api/MedicineCategory";
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(MedicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<MedicineCategory> medicineCategories = JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            return View(medicineCategories);
        }

        public async Task<ActionResult> Details(int id)
        {
            var medicine = await GetMedicineCategoryById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiseaseCategory diseaseCategory)
        {
            string data = JsonSerializer.Serialize(diseaseCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(MedicineCategoryApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(MedicineCategoryApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            MedicineCategory medicineCategory = JsonSerializer.Deserialize<MedicineCategory>(strData, options);

            return View(medicineCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, MedicineCategory medicineCategory)
        {
            string data = JsonSerializer.Serialize(medicineCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(MedicineCategoryApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<MedicineCategory> GetMedicineCategoryById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(MedicineCategoryApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<MedicineCategory>(strData, options);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var medicine = await GetMedicineCategoryById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await GetMedicineCategoryById(id);
            HttpResponseMessage response = await client.DeleteAsync(MedicineCategoryApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}