using EvergreenAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvergreenView.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiseaseController : Controller
    {
        
        private string DiseaseApiUrl = "";
        private string DiseaseCategoryApiUrl = "";
        private string ImageApiUrl = "";
        private readonly HttpClient client = null;

        public DiseaseController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            DiseaseApiUrl = "https://localhost:44334/api/Disease";
            DiseaseCategoryApiUrl = "https://localhost:44334/api/DiseaseCategory";
            ImageApiUrl = "https://localhost:44334/api/Image";
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
            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData, options);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            HttpResponseMessage responeImage = await client.GetAsync(ImageApiUrl);
            string strData1 = await responeImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Image> listImages = JsonSerializer.Deserialize<List<Image>>(strData1, options1);
            ViewData["Images"] = new SelectList(listImages, "ImageId", "AltText");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disease disease)
        {
            string data = JsonSerializer.Serialize(disease);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(DiseaseApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
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

            HttpResponseMessage responeImage = await client.GetAsync(ImageApiUrl);
            string strData1 = await responeImage.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Image> listImages = JsonSerializer.Deserialize<List<Image>>(strData1, options1);
            ViewData["Images"] = new SelectList(listImages, "ImageId", "AltText");

            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
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

            HttpResponseMessage responeImage = await client.GetAsync(ImageApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Image> listImages = JsonSerializer.Deserialize<List<Image>>(strData2, options2);
            ViewData["Images"] = new SelectList(listImages, "ImageId", "AltText");

            return View(disease);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Disease disease)
        {
            string data = JsonSerializer.Serialize(disease);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(DiseaseApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage responeDiseaseCategory = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData1 = await responeDiseaseCategory.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData1, options1);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");

            HttpResponseMessage responeImage = await client.GetAsync(ImageApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Image> listImages = JsonSerializer.Deserialize<List<Image>>(strData2, options2);
            ViewData["Images"] = new SelectList(listImages, "ImageId", "AltText");

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
            var disease = await GetDiseaseById(id);
            HttpResponseMessage response = await client.DeleteAsync(DiseaseApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
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

        public async Task<IEnumerable<Image>> GetImages()
        {
            HttpResponseMessage response = await client.GetAsync(ImageApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Image> listImage = JsonSerializer.Deserialize<List<Image>>(strData, options);
            return listImage;
        }

        public async Task SetViewData()
        {
            var listDiseaseCategory = await GetDiseaseCategory();
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");
            var listImage = await GetImages();
            ViewData["Images"] = new SelectList(listImage, "ImageId", "AltText");
        }
    }
}
