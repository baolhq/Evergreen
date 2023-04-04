using EvergreenAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Text;
using System.Net.Http.Json;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace EvergreenView.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly string _authApiUrl;
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(IHttpContextAccessor httpContextAccessor)
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
            _authApiUrl = "https://evergreen-api.onrender.com/api/auth";
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session
        {
            get { return _httpContextAccessor.HttpContext?.Session; }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (!string.IsNullOrWhiteSpace(Session.GetString("t")))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO account)
        {
            if (ModelState.IsValid)
            {
                if (account == null) return View();

                string data = JsonSerializer.Serialize(account);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{_authApiUrl}/login", content);
                await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "Cannot Log-In! Your account has been blocked!";
                    return RedirectToAction("Login", "Authentication");
                }

                var body = await response.Content.ReadFromJsonAsync<Account>();

                HttpContext.Session.SetString("n", body.Email);
                HttpContext.Session.SetString("r", body.Role);
                HttpContext.Session.SetString("t", body.Token);
                HttpContext.Session.SetString("i", body.AccountId.ToString());
                HttpContext.Session.SetString("a", body.AvatarUrl);

                return RedirectToAction("Index", HttpContext.Session.GetString("r") == "Admin" ? "Admin" : "Home");
            }

            TempData["error"] = "Your Email or Password is wrong!";
            return View("Login", account);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (!string.IsNullOrWhiteSpace(Session.GetString("t")))
            {

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountDTO account)
        {
            if (ModelState.IsValid)
            {
                if (account == null) return View();

                string data = JsonSerializer.Serialize(account);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{_authApiUrl}/register", content);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "Cannot Register! Email address already exists!";
                }
                else
                {
                    TempData["message"] = "Register success, Please visit your email to verify your account!!";
                    return RedirectToAction("VerifyAccount", "Authentication", new { email = account.Email });
                }
            }

            return View("Register", account);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult VerifyAccount(string email)
        {
            var verifyAccountDto = new VerifyAccountDTO()
            {
                Email = email
            };
            return View(verifyAccountDto);
        }

        [HttpPost]
        public async Task<ActionResult> VerifyAccount(VerifyAccountDTO verifyAccountDto)
        {
            var token = verifyAccountDto.Token;
            if (ModelState.IsValid)
            {
                string content = JsonSerializer.Serialize(token);
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{_authApiUrl}/verify?token={token}", data);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "Cannot Send Request!";
                }
                else
                {
                    TempData["message"] = "Your account is ready to use";
                    return RedirectToAction("Login", "Authentication");
                }
            }

            return View(verifyAccountDto);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (ModelState.IsValid)
            {
                string data = JsonSerializer.Serialize(email);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{_authApiUrl}/forgot-password?email={email}", content);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "Cannot Send Request!";
                }
                else
                {
                    string strData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    JsonSerializer.Deserialize<string>(strData, options);
                    TempData["message"] = "Please visit your email to reset password!";
                    return RedirectToAction("ResetPassword", "Authentication");
                }
            }

            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword(string tokenResetPassword)
        {
            var resetPasswordDto = new ResetPasswordDTO()
            {
                Token = tokenResetPassword
            };
            return View(resetPasswordDto);
        }


        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                string content = JsonSerializer.Serialize(resetPasswordDto);
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{_authApiUrl}/reset-password", data);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["error"] = "Cannot Reset Password!";
                }
                else
                {
                    TempData["message"] = "Your password has been reset successfully!";
                    return RedirectToAction("Login", "Authentication");
                }
            }

            return View(resetPasswordDto);
        }
    }
}