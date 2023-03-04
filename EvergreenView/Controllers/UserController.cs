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
        

        public UserController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            UserApiUrl = "https://localhost:44334/api/User";
            
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
        
        public async Task<ActionResult> Details()
        {
            if (HttpContext.Session.GetString("r") == null && HttpContext.Session.GetString("r") == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var email = HttpContext.Session.GetString("n");
            var user = await GetUser(email);
            if (user == null)
                return NotFound();
            return View(user);
        }
        
        public async Task<ActionResult> Edit(string email)
        {
            if (HttpContext.Session.GetString("r") == null && HttpContext.Session.GetString("r") == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(UserApiUrl + "/" + email);
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
        public async Task<ActionResult> Edit(string email, Account user)
        {
            if (HttpContext.Session.GetString("r") == null  && HttpContext.Session.GetString("r") == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string data = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(UserApiUrl + "/" + email, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details");
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            return View();

        }

        public async Task<ActionResult> Delete(string email)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var acc = await GetUser(email);
            if (acc == null)
                return NotFound();
            return View(acc);
        }

        private async Task<Account> GetUser(string email)
        {
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.GetAsync(UserApiUrl + "/" + email);
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
        public async Task<IActionResult> DeleteConfirmed(string email)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.DeleteAsync(UserApiUrl + "/" + email);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("AdminIndex");
            }
            return View();
        }



        public async Task<IActionResult> AdminIndex()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
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
    }
}
