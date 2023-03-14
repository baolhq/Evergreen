using EvergreenAPI.Models;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using NToastNotify;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EvergreenView.Controllers
{
    public class DiseaseController : Controller
    {

        private string DiseaseApiUrl = "";
        private string DiseaseCategoryApiUrl = "";
        private string ThumbnailApiUrl = "";
        private readonly IToastNotification _toastNotification;
        private readonly HttpClient client = null;

        public DiseaseController(IToastNotification toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            DiseaseApiUrl = "https://localhost:44334/api/Disease";
            DiseaseCategoryApiUrl = "https://localhost:44334/api/DiseaseCategory";
            ThumbnailApiUrl = "https://localhost:44334/api/Thumbnail";
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            
            HttpResponseMessage response = await client.GetAsync(DiseaseApiUrl);
           
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Disease> diseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);
            return View(diseases);
        }

        public async Task<ActionResult> Details(int id)
        {
            
            var disease = await GetDiseaseById(id);
            if (disease == null)
                return NotFound();
            return View(disease);
        }

        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData1 = await responeImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData1, options1);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disease disease)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(disease);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(DiseaseApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                _toastNotification.AddSuccessToastMessage("Create Disease Success!");
                return RedirectToAction("AdminIndex");
            }
            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData1 = await responeImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData1, options1);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            return View();
        }



        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await client.GetAsync(DiseaseApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Disease disease = JsonSerializer.Deserialize<Disease>(strData, options);

            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData1 = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData1, options1);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View(disease);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Disease disease)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(disease);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(DiseaseApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                _toastNotification.AddSuccessToastMessage("Update Disease Success!");
                return RedirectToAction("AdminIndex");
            }

            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData1 = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData1, options1);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

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

        private async Task<Disease> GetDiseaseById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(DiseaseApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();
             
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Disease>(strData, options);
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }
            var disease = await GetDiseaseById(id);
            if (disease == null)
                return NotFound();
            await SetViewData();
            return View(disease);
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

            var disease = await GetDiseaseById(id);
            HttpResponseMessage response = await client.DeleteAsync(DiseaseApiUrl + "/" + id);

            if (response.IsSuccessStatusCode)
            {
                _toastNotification.AddSuccessToastMessage("Delete Disease Success!");
                return RedirectToAction("AdminIndex");
                
            }
            return View(disease);
        }

        public async Task<IEnumerable<DiseaseCategory>> GetDiseaseCategory()
        {
            HttpResponseMessage response = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            return listDiseaseCategory;
        }




        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await client.GetAsync(ThumbnailApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImage = JsonSerializer.Deserialize<List<Thumbnail>>(strData, options);
            return listImage;
        }




        public async Task SetViewData()
        {
            var listDiseaseCategory = await GetDiseaseCategory();
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");
            var listImage = await GetImages();
            ViewData["Thumbnails"] = new SelectList(listImage, "ThumbnailId", "AltText");
        }






        public async Task<IActionResult> AdminIndex(string searchString)
        { 
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            string query = null;
            if (searchString != null)
                query =  "/Search" + "?search=" + searchString;
              

            HttpResponseMessage response; 
            if(query == null)
            {
                response = await client.GetAsync(DiseaseApiUrl);
            }
            else
            {
                response = await client.GetAsync(DiseaseApiUrl + query);
            }
            
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Disease> diseases = JsonSerializer.Deserialize<List<Disease>>(strData, options);
            return View(diseases);
        }





        public async Task<IActionResult> AdminDetails( int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var disease = await GetDiseaseById(id);
            if (disease == null)
                return NotFound();
            return View(disease);
        }
    }
}