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

namespace EvergreenView.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient client;
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

        public async Task<ActionResult> Details(int id)

        {
            if (HttpContext.Session.GetString("r") == null && HttpContext.Session.GetString("r") == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (!IsCurrentUser(id)) return BadRequest();

            var response = await client.GetAsync($"{UserApiUrl}/({id})");
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
                Professions = (string)temp["professions"]
            };

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }





        [Authorize("User")]
        public async Task<ActionResult> Edit(int id)

        {
            if (HttpContext.Session.GetString("r") == null && HttpContext.Session.GetString("r") == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            if (!IsCurrentUser(id)) return BadRequest();

            var response = await client.GetAsync($"{UserApiUrl}/({id})");
            string strData = await response.Content.ReadAsStringAsync();

            var temp = JObject.Parse(strData);

            if(temp == null)
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
        [Authorize("User")]
        public async Task<ActionResult> Edit(int id, Account user)
        {
            if (HttpContext.Session.GetString("r") == null && HttpContext.Session.GetString("r") == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var token = HttpContext.Session.GetString("t");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

           if(!IsCurrentUser(user.AccountId)) return BadRequest();

            var userEdit = new Account()
            {
                AccountId = id,
                Email = user.Email,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                Professions = user.Professions,
                Role = "User"
            };
            var userToEdit = JsonSerializer.Serialize(userEdit);
            HttpContent content = new StringContent(userToEdit, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(UserApiUrl + "/" + userEdit.AccountId, content);
            if (!response.IsSuccessStatusCode)
            {
                return View(user);
            }
            return RedirectToAction("Details", "User", new {Id = user.AccountId});
        }




        private bool IsCurrentUser(int id)
        {
            string currentUserId = HttpContext.Session.GetString("id");
            if (currentUserId != id.ToString()) return false;
            return true;
        }


        public async Task<ActionResult> Delete(int id)
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



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
                return RedirectToAction("AdminIndex");
            }
            return View();
        }


        [Authorize("Admin")]
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
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

            return View(listUsers);
        }

        [HttpPost]
        public async Task<IActionResult> ManageRole([FromBody] List<Account> accounts)
        {
            return RedirectToAction("ManageRole");
        }
    }
}
