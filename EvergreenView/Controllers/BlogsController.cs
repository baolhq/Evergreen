using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using AutoMapper.Execution;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace EvergreenView.Controllers
{

    public class BlogsController : Controller
    {
        
        private string BlogApiUrl = "";
        private string ThumbnailApiUrl = "";
        private readonly HttpClient client = null;
        private readonly IToastNotification _toastNotification;

        public BlogsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IToastNotification toastNotification)
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BlogApiUrl = "https://evergreen-api.onrender.com/api/Blog";
            ThumbnailApiUrl = "https://evergreen-api.onrender.com/api/Thumbnail";
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {



            HttpResponseMessage response = await client.GetAsync(BlogApiUrl);

            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Blog> listBlogs = JsonSerializer.Deserialize<List<Blog>>(strData, options);
            return View(listBlogs);
        }



        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var blog = await GetBlogById(id);
            if (blog == null)
                return NotFound();
            return View(blog);
        }

        public async Task<ActionResult> Read(int id)
        {
            var blog = await GetBlogById(id);
            if (blog == null)
                return NotFound();
            return View(blog);
        }

        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
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
        public async Task<IActionResult> Create(Blog p)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }


            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(p);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(BlogApiUrl, content).Result;
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
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage response = await client.GetAsync(BlogApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Blog blog = JsonSerializer.Deserialize<Blog>(strData, options);
            HttpResponseMessage responeImage = await client.GetAsync(ThumbnailApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Thumbnail> listImages = JsonSerializer.Deserialize<List<Thumbnail>>(strData2, options2);
            ViewData["Thumbnails"] = new SelectList(listImages, "ThumbnailId", "AltText");

            return View(blog);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Blog blog)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(blog);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(BlogApiUrl + "/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Update Successfully";
                return RedirectToAction("AdminIndex");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

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
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var blog = await GetBlogById(id);
            if (blog == null)
                return NotFound();
            await SetViewData();
            return View(blog);
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

            var blog = await GetBlogById(id);

            HttpResponseMessage response = await client.DeleteAsync(BlogApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }
            return View(blog);
        }



        private async Task<Blog> GetBlogById(int id)
        {
            HttpResponseMessage response = await client.GetAsync(BlogApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Blog>(strData, options);
        }

        public async Task<IEnumerable<Thumbnail>> GetImages()
        {
            HttpResponseMessage response = await client.GetAsync(BlogApiUrl);
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

            var listImage = await GetImages();
            ViewData["Thumbnails"] = new SelectList(listImage, "ThumbnailId", "AltText");
        }





        public async Task<IActionResult> AdminIndex(string searchString)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            string query = null;
            if (searchString != null)
                query = "/Search" + "?search=" + searchString;


            HttpResponseMessage response;
            if (query == null)
            {
                response = await client.GetAsync(BlogApiUrl);
            }
            else
            {
                response = await client.GetAsync(BlogApiUrl + query);
            }


            
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Blog> listBlogs = JsonSerializer.Deserialize<List<Blog>>(strData, options);
            return View(listBlogs);
        }
    }
}
