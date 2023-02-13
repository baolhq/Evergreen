using EvergreenAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvergreenView.Controllers
{
    public class ExpertController : Controller
    {
        private readonly HttpClient client = null;
        private string ExpertApiUrl = "";
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpertController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            ExpertApiUrl = "https://localhost:44334/api/Expert";
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

            HttpResponseMessage response = await client.GetAsync(ExpertApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listExperts = JsonSerializer.Deserialize<List<Account>>(strData, options);
            return View(listExperts);

        }



        public async Task<ActionResult> Details()
        {
            if (HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index");
            }

            var username = HttpContext.Session.GetString("n");
            var user = await GetExpert(username);
            if (user == null)
                return NotFound();
            return View(user);
        }





        public async Task<ActionResult> Edit(string username)
        {
            if (HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(ExpertApiUrl + "/" + username);
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
        public async Task<ActionResult> Edit(string username, Account expert)
        {
            if (HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(expert);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(ExpertApiUrl + "/" + username, content);
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


        public async Task<ActionResult> Delete(string username)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var acc = await GetExpert(username);
            if (acc == null)
                return NotFound();
            return View(acc);
        }



        private async Task<Account> GetExpert(string username)
        {
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(ExpertApiUrl + "/" + username);
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
        public async Task<IActionResult> DeleteConfirmed(string username)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.DeleteAsync(ExpertApiUrl + "/" + username);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


    }
}
