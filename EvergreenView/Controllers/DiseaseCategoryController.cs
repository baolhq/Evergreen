﻿using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EvergreenAPI.Models;
using AutoMapper.Execution;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EvergreenView.Controllers
{
    
    public class DiseaseCategoryController : Controller
    {
        private string DiseaseCategoryApiUrl = "";
        private readonly HttpClient client = null;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public DiseaseCategoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            DiseaseCategoryApiUrl = "https://localhost:44334/api/DiseaseCategory";
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
            HttpResponseMessage response = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<DiseaseCategory> diseaseCategories = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            return View(diseaseCategories);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "Blogs");
            }
            var member = await GetDiseaseCategoryById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        public ActionResult Create()
        {
            if (session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "DiseaseCategory");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiseaseCategory diseaseCategory)
        {
            if (session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "DiseaseCategory");
            }
            string data = JsonSerializer.Serialize(diseaseCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(DiseaseCategoryApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "DiseaseCategory");
            }
            HttpResponseMessage response = await client.GetAsync(DiseaseCategoryApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            DiseaseCategory diseaseCategory = JsonSerializer.Deserialize<DiseaseCategory>(strData, options);

            return View(diseaseCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, DiseaseCategory diseaseCategory)
        {
            if (session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "DiseaseCategory");
            }
            string data = JsonSerializer.Serialize(diseaseCategory);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(DiseaseCategoryApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<DiseaseCategory> GetDiseaseCategoryById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(DiseaseCategoryApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<DiseaseCategory>(strData, options);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "DiseaseCategory");
            }
            var member = await GetDiseaseCategoryById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "DiseaseCategory");
            }
            var member = await GetDiseaseCategoryById(id);
            HttpResponseMessage response = await client.DeleteAsync(DiseaseCategoryApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
