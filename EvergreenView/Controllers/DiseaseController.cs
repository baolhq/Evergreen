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
using System.Linq;

namespace EvergreenView.Controllers
{
    public class DiseaseController : Controller
    {

        private string DiseaseApiUrl = "";
        private string DiseaseCategoryApiUrl = "";
        private string ThumbnailApiUrl = "";
        private string MedicineApiUrl = "";
        private string TreatmentApiUrl = "";
        private readonly IToastNotification _toastNotification;
        private readonly HttpClient client = null;

        public DiseaseController(IToastNotification toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            DiseaseApiUrl = "https://evergreen-api.onrender.com/api/Disease";
            DiseaseCategoryApiUrl = "https://evergreen-api.onrender.com/api/DiseaseCategory";
            ThumbnailApiUrl = "https://evergreen-api.onrender.com/api/Thumbnail";
            MedicineApiUrl = "https://evergreen-api.onrender.com/api/Medicine";
            TreatmentApiUrl = "https://evergreen-api.onrender.com/api/Treatment";

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


            HttpResponseMessage responeMedicine = await client.GetAsync(MedicineApiUrl);
            string strData2 = await responeMedicine.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData2, options2);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");


            HttpResponseMessage responeTreatment = await client.GetAsync(TreatmentApiUrl);
            string strData3 = await responeTreatment.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData3, options3);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "Method");

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
                TempData["message"] = "Create Successfully";
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



            HttpResponseMessage responeMedicine = await client.GetAsync(MedicineApiUrl);
            string strData2 = await responeMedicine.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData2, options2);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");



            HttpResponseMessage responeTreatment = await client.GetAsync(TreatmentApiUrl);
            string strData3 = await responeTreatment.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData3, options3);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "Method");

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


            HttpResponseMessage responeMedicine = await client.GetAsync(MedicineApiUrl);
            string strData3 = await responeMedicine.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData3, options3);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");


            HttpResponseMessage responeTreatment = await client.GetAsync(TreatmentApiUrl);
            string strData4 = await responeTreatment.Content.ReadAsStringAsync();
            var options4 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData4, options4);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "Method");


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
            


            response = await client.GetAsync(DiseaseCategoryApiUrl);
            string strData1 = await response.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<DiseaseCategory> listDiseaseCategory = JsonSerializer.Deserialize<List<DiseaseCategory>>(strData1, options1);
            ViewData["DiseaseCategories"] = new SelectList(listDiseaseCategory, "DiseaseCategoryId", "Name");





            response = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await response.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {

                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");




            response = await client.GetAsync(MedicineApiUrl);
            string strData3 = await response.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {

                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData3, options3);
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");




            response = await client.GetAsync(TreatmentApiUrl);
            string strData4 = await response.Content.ReadAsStringAsync();
            var options4 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData4, options4);
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "Method");




            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("AdminIndex");
            }

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
                TempData["message"] = "Delete Successfully";
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


        public async Task<IEnumerable<Medicine>> GetMedicine()
        {
            HttpResponseMessage response = await client.GetAsync(MedicineApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Medicine> listMedicine = JsonSerializer.Deserialize<List<Medicine>>(strData, options);
            return listMedicine;
        }



        public async Task<IEnumerable<Treatment>> GetTreatment()
        {
            HttpResponseMessage response = await client.GetAsync(TreatmentApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Treatment> listTreatment = JsonSerializer.Deserialize<List<Treatment>>(strData, options);
            return listTreatment;
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

            var listTreatment = await GetTreatment();
            ViewData["Treatments"] = new SelectList(listTreatment, "TreatmentId", "Method");
            var listMedicine = await GetMedicine();
            ViewData["Medicines"] = new SelectList(listMedicine, "MedicineId", "Name");
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
                query = "/Search" + "?search=" + searchString;


            HttpResponseMessage response;
            if (query == null)
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





        public async Task<IActionResult> AdminDetails(int id)
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