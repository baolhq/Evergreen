﻿using Microsoft.AspNetCore.Http;
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

namespace EvergreenView.Controllers
{
    public class BlogsController : Controller
    {
        private readonly HttpClient client = null;
        private string BlogApiUrl = "";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public BlogsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            BlogApiUrl = "https://localhost:44395/api/Blog";
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


        public async Task <IActionResult> Index()
        {
            if (session.GetString("User") == null || session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            string query = null;
            HttpResponseMessage response = await client.GetAsync(BlogApiUrl + query);
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
            if (session.GetString("User") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var blog = await GetBlogByIdAsync(id);
            if (blog == null)
                return NotFound();
            return View(blog);
        }



        public async Task<ActionResult> Create()
        {
            if (session.GetString("User") == null || session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            HttpResponseMessage respone = await client.GetAsync(BlogApiUrl);
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog p)
        {
            string data = JsonSerializer.Serialize(p);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(BlogApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return View();


        }




        public async Task<ActionResult> Delete(int id)
        {
            if (session.GetString("User") == null || session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var blog = await GetBlogByIdAsync(id);
            if (blog == null)
                return NotFound();
            return View(blog);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await GetBlogByIdAsync(id);

            HttpResponseMessage response = await client.DeleteAsync(BlogApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }



        private async Task<Blog> GetBlogByIdAsync(int id)
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







        public async Task<ActionResult> Edit(int id)
        {
            if (session.GetString("User") == null || session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            HttpResponseMessage response = await client.GetAsync(BlogApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Blog blog = JsonSerializer.Deserialize<Blog>(strData, options);

            return View(blog);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Blog blog)
        {
            string data = JsonSerializer.Serialize(blog);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(BlogApiUrl + "/" + id, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            return View();

        }

    }
}
