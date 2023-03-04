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

namespace EvergreenView.Controllers
{

    public class BlogsController : Controller
    {
        
        private string BlogApiUrl = "";
        private string ImageApiUrl = "";
        private readonly HttpClient client = null;

        public BlogsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BlogApiUrl = "https://localhost:44334/api/Blog";
            ImageApiUrl = "https://localhost:44334/api/Image";
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
                return RedirectToAction("AdminIndex");
            }

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
            HttpResponseMessage responeImage = await client.GetAsync(ImageApiUrl);
            string strData2 = await responeImage.Content.ReadAsStringAsync();
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Image> listImages = JsonSerializer.Deserialize<List<Image>>(strData2, options2);
            ViewData["Images"] = new SelectList(listImages, "ImageId", "AltText");

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
                return RedirectToAction("AdminIndex");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

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

        public async Task<IEnumerable<Image>> GetImages()
        {
            HttpResponseMessage response = await client.GetAsync(BlogApiUrl);
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

            var listImage = await GetImages();
            ViewData["Images"] = new SelectList(listImage, "ImageId", "AltText");
        }

        public async Task<IActionResult> AdminIndex()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            
            HttpResponseMessage response = await client.GetAsync(BlogApiUrl);
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
