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

namespace EvergreenView.Controllers
{

    public class MedicineController : Controller
    {
        private string ImageApiUrl = "";
        private string MedicineApiUrl = "";
        private string MedicineCategoryApiUrl = "";
        private string DiseaseApiUrl = "";
        private readonly HttpClient client = null;

        public MedicineController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MedicineApiUrl = "https://localhost:44334/api/Medicine";
            MedicineCategoryApiUrl = "https://localhost:44334/api/MedicineCategory";
            ImageApiUrl = "https://localhost:44334/api/Image";
            DiseaseApiUrl = "https://localhost:44334/api/Disease";
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

            HttpResponseMessage responeImage1 = await client.GetAsync(ImageApiUrl);
            string strData3 = await responeImage1.Content.ReadAsStringAsync();
            var options3 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Image> listImages = JsonSerializer.Deserialize<List<Image>>(strData3, options3);
            ViewData["Images"] = new SelectList(listImages, "ImageId", "AltText");
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
            ViewData["Images"] = new SelectList(listImage, "ImageId", "AltText");
        }



        public async Task<IEnumerable<Image>> GetImages()
        {
            HttpResponseMessage response = await client.GetAsync(MedicineApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Image> listImage = JsonSerializer.Deserialize<List<Image>>(strData, options);
            return listImage;
        }

        public async Task<IActionResult> AdminIndex()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await client.GetAsync(MedicineApiUrl);
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
