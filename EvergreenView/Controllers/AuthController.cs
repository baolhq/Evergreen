using EvergreenAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> Login(AccountDTO account)
        {
            if (account == null) return View();

            string data = JsonSerializer.Serialize(account);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = client.PostAsync($"{AuthApiUrl}/login", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadFromJsonAsync<Account>();

                HttpContext.Session.SetString("n", body.Username);
                HttpContext.Session.SetString("r", body.Role);
                HttpContext.Session.SetString("t", body.Token);

                return RedirectToAction("Index", "Home");
            }

            ViewData["error"] = "Username or password is incorrect";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(AccountDTO account)
        {
            if (account == null) return View();

            string data = JsonSerializer.Serialize(account);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = client.PostAsync($"{AuthApiUrl}/register", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            var err = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
            ViewData["error"] = err;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View();
        }
    }
}
