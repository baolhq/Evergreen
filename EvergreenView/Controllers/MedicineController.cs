﻿using EvergreenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NToastNotify;

namespace EvergreenView.Controllers
{

    public class MedicineController : Controller
    {
        private string ThumbnailApiUrl = "";
        private string MedicineApiUrl = "";
        private string MedicineCategoryApiUrl = "";
        private string DiseaseApiUrl = "";
        private readonly HttpClient client = null;
        private readonly IToastNotification _toastNotification;

        public MedicineController(IToastNotification toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MedicineApiUrl = "https://evergreen-api.onrender.com/api/Medicine";
            MedicineCategoryApiUrl = "https://evergreen-api.onrender.com/api/MedicineCategory";
            ThumbnailApiUrl = "https://evergreen-api.onrender.com/api/Thumbnail";
            DiseaseApiUrl = "https://evergreen-api.onrender.com/api/Disease";
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {

            
            HttpResponseMessage response = await client.GetAsync(MedicineApiUrl);
            
           
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(strData, options);
            return View(medicines);
        }

        public async Task<ActionResult> Details(int id)
        {
            var medicine = await GetMedicineById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);
        }

        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeMedicineCategory = await client.GetAsync(MedicineCategoryApiUrl);
            string strData = await responeMedicineCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory = JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");


            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData1 = await responeDisease.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData1, options1);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");



            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicine medicine)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(medicine);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(MedicineApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Create Successfully";
                return RedirectToAction("AdminIndex");
            }
            HttpResponseMessage responeMedicineCategory = await client.GetAsync(MedicineCategoryApiUrl);
            string strData = await responeMedicineCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory = JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");


            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData1 = await responeDisease.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData1, options1);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");

            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await client.GetAsync(MedicineApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Medicine medicine = JsonSerializer.Deserialize<Medicine>(strData, options);

            HttpResponseMessage responeMedicineCategory = await client.GetAsync(MedicineCategoryApiUrl);
            string strData1 = await responeMedicineCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory = JsonSerializer.Deserialize<List<MedicineCategory>>(strData1, options1);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");


            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData2 = await responeDisease.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData2, options2);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");

            HttpResponseMessage responeImage1 = await client.GetAsync(ThumbnailApiUrl);
            string strData3 = await responeImage1.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData3, options3);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View(medicine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Medicine medicine)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(medicine);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(MedicineApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("AdminIndex");
            }

            HttpResponseMessage responeMedicineCategory = await client.GetAsync(MedicineCategoryApiUrl);
            string strData = await responeMedicineCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory = JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");


            HttpResponseMessage responeDisease = await client.GetAsync(DiseaseApiUrl);
            string strData1 = await responeDisease.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Disease> listDiseases = JsonSerializer.Deserialize<List<Disease>>(strData1, options1);
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");


            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }
            var medicine = await GetMedicineById(id);
            if (medicine == null)
                return NotFound();
            await SetViewData();
            return View(medicine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var medicine = await GetMedicineById(id);
            HttpResponseMessage response = await client.DeleteAsync(MedicineApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }
            return View(medicine);
        }

        private async Task<Medicine> GetMedicineById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(MedicineApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Medicine>(strData, options);
        }

        public async Task<IEnumerable<MedicineCategory>> GetMedicineCategories()
        {
            HttpResponseMessage response = await client.GetAsync(MedicineCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<MedicineCategory> listMedicineCategory = JsonSerializer.Deserialize<List<MedicineCategory>>(strData, options);
            return listMedicineCategory;
        }


        public async Task<IEnumerable<Disease>> GetDiseases()
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
            var listDiseases = await GetDiseases();
            ViewData["Diseases"] = new SelectList(listDiseases, "DiseaseId", "Name");
            var listMedicineCategory = await GetMedicineCategories();
            ViewData["MedicineCategories"] = new SelectList(listMedicineCategory, "MedicineCategoryId", "Name");
            var listImage = await GetImages();
            ViewData["Thumbnails"] = new SelectList(listImage, "ThumbnailId", "AltText");
        }



        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await client.GetAsync(MedicineApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImage = JsonSerializer.Deserialize<List<Thumbnail>>(strData, options);
            return listImage;
        }

        public async Task<IActionResult> AdminIndex(string searchString)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            string query = null;
            if (searchString != null)
                query = "/Search" + "?search=" + searchString;


            HttpResponseMessage response;
            if (query == null)
            {
                response = await client.GetAsync(MedicineApiUrl);
            }
            else
            {
                response = await client.GetAsync(MedicineApiUrl + query);
            }



            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Medicine> medicines = JsonSerializer.Deserialize<List<Medicine>>(strData, options);
            return View(medicines);
        }




        public async Task<IActionResult> AdminDetails(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var medicine = await GetMedicineById(id);
            if (medicine == null)
                return NotFound();
            return View(medicine);

        }
    }
}
