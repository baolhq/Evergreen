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
using NToastNotify;

namespace EvergreenView.Controllers
{
    
    public class TreatmentController : Controller
    {
        private string ThumbnailApiUrl = "";
        private string TreatmentApiUrl = "";
        
        private readonly HttpClient client = null;
        private readonly IToastNotification _toastNotification;

        public TreatmentController(IToastNotification toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            TreatmentApiUrl = "https://localhost:44334/api/Treatment";
            ThumbnailApiUrl = "https://localhost:44334/api/Thumbnail";
            _toastNotification = toastNotification;
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
            
            var treatment = await GetTreatmentById(id);
            if (treatment == null)
                return NotFound();
            return View(treatment);
        }






        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }
            

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
        public async Task<IActionResult> Create(Treatment treatment)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
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
                TempData["message"] = "Create Successfully";
                return RedirectToAction("AdminIndex");
            }
            

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
            HttpResponseMessage response = await client.GetAsync(TreatmentApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Treatment treatment = JsonSerializer.Deserialize<Treatment>(strData, options);

            

            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");
            return View(treatment);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Treatment treatment)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(treatment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(TreatmentApiUrl + "/" + id, content).Result;
            

            

            response = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await response.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("AdminIndex");
            }
            return View();
        }







        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
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
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var treatment = await GetTreatmentById(id);
            HttpResponseMessage response = await client.DeleteAsync(TreatmentApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }
            return View(treatment);
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

        

        public async Task SetViewData()
        {
            
            var listImage = await GetImages();
            ViewData["Thumbnails"] = new SelectList(listImage, "ThumbnailId", "AltText");
        }



        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await client.GetAsync(TreatmentApiUrl);
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
                response = await client.GetAsync(TreatmentApiUrl);
            }
            else
            {
                response = await client.GetAsync(TreatmentApiUrl + query);
            }

            
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Treatment> treatments = JsonSerializer.Deserialize<List<Treatment>>(strData, options);
            return View(treatments);
        }





        public async Task<IActionResult> AdminDetails(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin" && HttpContext.Session.GetString("r") != "Professor")
            {
                return RedirectToAction("Index");
            }

            var treatment = await GetTreatmentById(id);
            if (treatment == null)
                return NotFound();
            return View(treatment);

        }
    }
}
