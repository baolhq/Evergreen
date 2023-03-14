using EvergreenAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using EvergreenAPI.Models;
using System.Text;
using System.Net.Http.Json;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace EvergreenView.Controllers
{

    public class AuthenticationController : Controller
    {
        private string AuthApiUrl = "";
        private readonly HttpClient client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IToastNotification _toastNotification;


        public AuthenticationController(IHttpContextAccessor httpContextAccessor, IToastNotification toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            AuthApiUrl = "https://localhost:44334/api/auth";
            _toastNotification = toastNotification;


            _httpContextAccessor = httpContextAccessor;
        }
        public ISession session { get { return _httpContextAccessor.HttpContext.Session; } }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (!string.IsNullOrWhiteSpace(session.GetString("t")))
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

            string data = System.Text.Json.JsonSerializer.Serialize(account);
            var content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{AuthApiUrl}/login", content);
            string strData = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    ViewData["message"] = message.Substring(1, message.Length - 2);
                    _toastNotification.AddErrorToastMessage(message);
                    return RedirectToAction("Login", "Authentication");
                }
                else
                {
                    var body = await response.Content.ReadFromJsonAsync<Account>();

                    HttpContext.Session.SetString("n", body.Email);
                    HttpContext.Session.SetString("r", body.Role);
                    HttpContext.Session.SetString("t", body.Token);
                    HttpContext.Session.SetString("i", body.AccountId.ToString());
                    HttpContext.Session.SetString("a", body.AvatarUrl.ToString());

                    if (HttpContext.Session.GetString("r") == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            _toastNotification.AddErrorToastMessage("Your Email or Password is wrong!!");
            return View("Login", account);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            session.Clear();
            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (!string.IsNullOrWhiteSpace(session.GetString("t")))
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

                string data = System.Text.Json.JsonSerializer.Serialize(account);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{AuthApiUrl}/register", content);
                if (!response.IsSuccessStatusCode)
                {

                    var message = await response.Content.ReadAsStringAsync();
                    _toastNotification.AddErrorToastMessage($"{message}");
                    ViewData["message"] = message;

                }
                else 
                {
                    _toastNotification.AddSuccessToastMessage("Register success, please visit your email to verify your account!!");
                    return RedirectToAction("VerifyAccount", "Authentication", new {email = account.Email});
                }

            }
            return View("Register", account);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult VerifyAccount(string email)
        {
            var verifyaccountDTO = new VerifyAccountDTO()
            {
                Email = email
            };
            return View(verifyaccountDTO);
        }

        [HttpPost]
        public async Task<ActionResult> VerifyAccount(VerifyAccountDTO verifyAccountDTO)
        {
            var token = verifyAccountDTO.Token;
            if (ModelState.IsValid)
            {
                string content = System.Text.Json.JsonSerializer.Serialize(token);
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{AuthApiUrl}/verify?token={token}", data);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest("Something is wrong when trying to send request!");


                }
                else
                {
                    _toastNotification.AddSuccessToastMessage("Your Account Is Ready To Use!");
                    return RedirectToAction("Login", "Authentication");
                }
            }
            return View(verifyAccountDTO);

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
                string data = System.Text.Json.JsonSerializer.Serialize(email);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{AuthApiUrl}/forgot-password?email={email}", content);
                if (!response.IsSuccessStatusCode)
                {

                    var message = await response.Content.ReadAsStringAsync();
                    _toastNotification.AddErrorToastMessage(message);
                    ViewData["message"] = message.Substring(1, message.Length - 2);

                }
                else
                {
                    string strData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    string TokenToResetPassword = System.Text.Json.JsonSerializer.Deserialize<string>(strData, options);

                    var message = "Please visit your mail to reset your password!";
                    _toastNotification.AddSuccessToastMessage(message);
                    ViewData["message"] = message.Substring(1, message.Length - 2);
                    return RedirectToAction("ResetPassword", "Authentication");
                }
            }
            return View();
        }



       


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ResetPassword(string TokenResetPassword)
        {
            var resetPasswordDTO = new ResetPasswordDTO()
            {
                Token = TokenResetPassword
            };
            return View(resetPasswordDTO);
        }


        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            if (ModelState.IsValid)
            {

                string content = System.Text.Json.JsonSerializer.Serialize(resetPasswordDTO);
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{AuthApiUrl}/reset-password", data);
                if (!response.IsSuccessStatusCode)
                {

                    var message = await response.Content.ReadAsStringAsync();
                    _toastNotification.AddErrorToastMessage(message);
                    ViewData["message"] = message.Substring(1, message.Length - 2);

                }
                else
                {
                    _toastNotification.AddSuccessToastMessage("Your Password Reseted successfully!");
                    return RedirectToAction("Login", "Authentication");
                }
            }
            
            return View(resetPasswordDTO);
        }
         
    }
}
