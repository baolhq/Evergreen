using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using EvergreenAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace EvergreenView.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client = null;
        private string UserApiUrl = "";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            UserApiUrl = "https://localhost:44334/api/User";
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

            var token = HttpContext.Session.GetString("t");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listUsers = JsonSerializer.Deserialize<List<Account>>(strData, options);
            return View(listUsers);

        }
        
        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var user = await GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return View(user);
        }




       
        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage respone = await client.GetAsync(UserApiUrl);
            string strData = await respone.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return View();
        }



       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account user)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string data = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(UserApiUrl, content);
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





        
        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(UserApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Account user = JsonSerializer.Deserialize<Account>(strData, options);

            return View(user);
        }




        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Account user)
        {
            if (HttpContext.Session.GetString("r") == null )
            {
                return RedirectToAction("Index");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string data = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UserApiUrl + "/" + id, content);
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
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var product = await GetUserByIdAsync(id);
            if (product == null)
                return NotFound();
            return View(product);
        }









        private async Task<Account> GetUserByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync(UserApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Account>(strData, options);
        }









        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var user = await GetUserByIdAsync(id);

            HttpResponseMessage response = await client.DeleteAsync(UserApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
