using EvergreenAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;

namespace EvergreenView.Controllers
{
    public class AuthController : Controller
    {
        private string AuthApiUrl = "";
        private readonly HttpClient client = null;

        public AuthController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AuthApiUrl = "https://localhost:44334/api/auth";
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountDTO account)
        {
            if (account == null) return View();

            string data = JsonSerializer.Serialize(account);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = client.PostAsync($"{AuthApiUrl}/login", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
