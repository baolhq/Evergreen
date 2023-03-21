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
using Newtonsoft.Json.Linq;
using System;
using EvergreenAPI.DTO;
using System.Net.Http.Json;
using NToastNotify;

namespace EvergreenView.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client;
        private string UserApiUrl = "";
        private readonly IToastNotification _toastNotification;


        public UserController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IToastNotification toastNotification)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            UserApiUrl = "https://localhost:44334/api/User";
            _toastNotification = toastNotification;

        }

        public async Task<ActionResult> AdminDetails(int id)
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

            var response = await client.GetAsync($"{UserApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("Not Found");
            }

            var user = new Account()
            {
                AccountId = id,
                Email = (string)temp["email"],
                Username = (string)temp["username"],
                PhoneNumber = (string)temp["phoneNumber"],
                FullName = (string)temp["fullName"],
                Role = (string)temp["role"],
                AvatarUrl = (string)temp["avatarUrl"],
                VerifiedAt = DateTime.Parse((string)temp["verifiedAt"]),
                Professions = (string)temp["professions"]
            };

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!IsCurrentUser(id)) return BadRequest();

            var token = HttpContext.Session.GetString("t");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{UserApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("Not Found");
            }

            var user = new Account()
            {
                AccountId = id,
                Email = (string)temp["email"],
                Username = (string)temp["username"],
                PhoneNumber = (string)temp["phoneNumber"],
                FullName = (string)temp["fullName"],
                AvatarUrl = (string)temp["avatarUrl"],
                VerifiedAt = DateTime.Parse((string)temp["verifiedAt"]),
                Professions = (string)temp["professions"],
                Bio = (string)temp["bio"],
            };

            return View(user);
        }

        public async Task<ActionResult> AdminEdit(int id)

        {
            if (HttpContext.Session.GetString("r") == null || HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{UserApiUrl}/({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("Not Found");
            }


            var user = new Account()
            {
                Email = (string)temp["email"],
                Username = (string)temp["username"],
                PhoneNumber = (string)temp["phoneNumber"],
                Professions = (string)temp["professions"]
            };

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> AdminEdit(int id, Account user)
        {
            if (HttpContext.Session.GetString("r") == null || HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userEdit = new Account()
            {
                AccountId = id,
                Email = user.Email,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Professions = user.Professions,
                FullName = user.FullName,
                Bio = user.Bio,
                Role = "User"
            };
            var userToEdit = JsonSerializer.Serialize(userEdit);
            var content = new StringContent(userToEdit, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(UserApiUrl + "/" + userEdit.AccountId, content);
            if (!response.IsSuccessStatusCode)
            {

                return View(user);
            }
            TempData["message"] = "Update Successfully";
            return RedirectToAction("Details", "User", new { Id = user.AccountId });
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("r") == null) return RedirectToAction("Index", "Home");

            if (!IsCurrentUser(id)) return NotFound();

            var token = HttpContext.Session.GetString("t");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Index", "Home");

            token = token.Replace("\"", string.Empty);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{UserApiUrl}/{id}");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if (temp == null)
            {
                return View("Not Found");
            }

            var user = new Account()
            {
                AccountId = id,
                Email = (string)temp["email"],
                Username = (string)temp["username"],
                PhoneNumber = (string)temp["phoneNumber"],
                FullName = (string)temp["fullName"],
                AvatarUrl = (string)temp["avatarUrl"],
                VerifiedAt = DateTime.Parse((string)temp["verifiedAt"]),
                Professions = (string)temp["professions"],
                Bio = (string)temp["bio"],
            };

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Edit(int id, Account user)
        {
            if (HttpContext.Session.GetString("r") != "User")
                return RedirectToAction("Index", "Home");

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userEdit = new AccountUpdateDTO()
            {
                AccountId = id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
            };
            var userToEdit = JsonSerializer.Serialize(userEdit);
            var content = new StringContent(userToEdit, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(UserApiUrl + "/" + id, content);
            if (!response.IsSuccessStatusCode)
            {
                return View(user);
            }
            TempData["message"] = "Update Successfully";
            return RedirectToAction("Details", "User", new { Id = id });
        }

        private bool IsCurrentUser(int id)
        {
            string currentUserId = HttpContext.Session.GetString("i");
            if (currentUserId != id.ToString()) return false;
            return true;
        }


       /* public async Task<ActionResult> AdminDelete(int id)
        {

            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new Account();
            HttpResponseMessage responseUser = await client.GetAsync(UserApiUrl + "/" + id);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (responseUser.IsSuccessStatusCode)
            {
                string userData = await responseUser.Content.ReadAsStringAsync();
                model = JsonSerializer.Deserialize<Account>(userData, options);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage getUser = await client.GetAsync(UserApiUrl + "/" + id);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string strData = await getUser.Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<Account>(strData, options);

            HttpResponseMessage response = await client.DeleteAsync(UserApiUrl + "/" + user.AccountId);
            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Delete Successfully";
                return RedirectToAction("AdminIndex");
            }
            return View();
        }
*/





        public async Task<IActionResult> AdminIndex(string searchString)

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

            string query = null;
            if (searchString != null)
                query = "/Search" + "?search=" + searchString;


            HttpResponseMessage response;
            if (query == null)
            {
                response = await client.GetAsync(UserApiUrl);
            }
            else
            {
                response = await client.GetAsync(UserApiUrl + query);
            }


            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listUsers = JsonSerializer.Deserialize<List<Account>>(strData, options);
            return View(listUsers);
        }

        public IActionResult AdminCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminCreate(Account account)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formUser = new UserDTO()
            {
                Email = account.Email,
                FullName = account.FullName,
                Username = account.Username,
                PhoneNumber = account.PhoneNumber,
                Professions = account.Professions,
                Role = account.Role,
                Password = account.Password,
                ConfirmPassword = account.Password
            };
            var newUser = JsonSerializer.Serialize(formUser);
            var content = new StringContent(newUser, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(UserApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                return View(account);
            }
            TempData["message"] = "Create Successfully";
            return RedirectToAction("AdminIndex");
        }

        [HttpGet]
        public async Task<IActionResult> ManageRole()
        {
            var token = HttpContext.Session.GetString("t");
            if (HttpContext.Session.GetString("r") != "Admin" || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get users
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listUsers = JsonSerializer.Deserialize<List<Account>>(strData, options);

            TempData["message"] = "Manage Role Successfully";
            return View(listUsers);
        }



        [HttpGet]
        public async Task<IActionResult> ManageBlocked()
        {
            var token = HttpContext.Session.GetString("t");
            if (HttpContext.Session.GetString("r") != "Admin" || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            token = token.Replace("\"", string.Empty);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get users
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Account> listUsers = JsonSerializer.Deserialize<List<Account>>(strData, options);

            TempData["message"] = "Manage Status Successfully";
            return View(listUsers);

        }

    }
}
