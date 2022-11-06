using EvergreenAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvergreenView.Controllers.Admin
{
    public class MessageController : Controller
    {
        private readonly HttpClient client = null;
        private string MessageApiUrl = "";
        private string AccountApiUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessageController(IHttpContextAccessor httpContextAccessor)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MessageApiUrl = "https://localhost:44334/api/Message";
            AccountApiUrl = "https://localhost:44334/api/Account";
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
            
            string query = null;
           
            HttpResponseMessage response = await client.GetAsync(MessageApiUrl + query);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Message> listMessage = JsonSerializer.Deserialize<List<Message>>(strData, options);
            return View(listMessage);

        }

        
        public async Task<ActionResult> Details(int id)
        {
           
            var mess = await GetMessageByIdAsync(id);
            if (mess == null)
                return NotFound();
            return View(mess);
        }




        
        public async Task<ActionResult> UserCreate()
        {
            if (HttpContext.Session.GetString("r") == "Admin" || HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string strData = await responeAccount.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return View();
        }



       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserCreate(Message m)
        {
            if (HttpContext.Session.GetString("r") == "Admin" || HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Index");
            }
            string data = JsonSerializer.Serialize(m);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(MessageApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string strData = await responeAccount.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            
            return View();


        }


        
        public async Task<ActionResult> AdminCreate()
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string strData = await responeAccount.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Account> listAccount = JsonSerializer.Deserialize<List<Account>>(strData, options);
            ViewData["Accounts"] = new SelectList(listAccount, "AccountId", "Fullname");
            return View();
        }



        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate(Message m)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            string data = JsonSerializer.Serialize(m);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(MessageApiUrl, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string strData = await responeAccount.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Account> listAccount = JsonSerializer.Deserialize<List<Account>>(strData, options);
            ViewData["Accounts"] = new SelectList(listAccount, "AccountId", "Fullname");
            return View();


        }




        
        public async Task<ActionResult> AdminEdit(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage response = await client.GetAsync(MessageApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Message message = JsonSerializer.Deserialize<Message>(strData, options);

            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string str = await responeAccount.Content.ReadAsStringAsync();

            List<Account> listAccount = JsonSerializer.Deserialize<List<Account>>(str, options);
            ViewData["Accounts"] = new SelectList(listAccount, "AccountId", "Fullname");
            return View(message);
        }




        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminEdit(int id, Message product)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }
            string data = JsonSerializer.Serialize(product);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(MessageApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string strData = await responeAccount.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Account> listAccounts = JsonSerializer.Deserialize<List<Account>>(strData, options);
            ViewData["Accounts"] = new SelectList(listAccounts, "AccountId", "Fullname");
            return View();

        }


        
        public async Task<ActionResult> UserEdit(int id)
        {
            if (HttpContext.Session.GetString("r") == "Admin" || HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Index");
            }
            var accountId = session.GetString("AccountId");
           if (accountId == null)
            {
                return RedirectToAction("Index", "Home");
            }
           
           

            HttpResponseMessage response = await client.GetAsync(MessageApiUrl + "/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Message message = JsonSerializer.Deserialize<Message>(strData, options);

            if(accountId != message.AccountId.ToString())
            {
                return RedirectToAction("Index", "Home");
            }

            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string str = await responeAccount.Content.ReadAsStringAsync();

            
            return View(message);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(int id, Message message)
        {
            if (HttpContext.Session.GetString("r") == "Admin" || HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Index");
            }
            var accountId = session.GetString("AccountId");
            if (accountId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (accountId != message.AccountId.ToString())
            {
                return RedirectToAction("Index", "Home");
            }
            string data = JsonSerializer.Serialize(message);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(MessageApiUrl + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            HttpResponseMessage responeAccount = await client.GetAsync(AccountApiUrl);
            string strData = await responeAccount.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            
            return View();

        }



        
        public async Task<ActionResult> UserDelete(int id)
        {
            if (HttpContext.Session.GetString("r") == "Admin" || HttpContext.Session.GetString("User") == null)
            {
                return RedirectToAction("Index");
            }
            var accountId = session.GetString("AccountId");
            if (accountId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var mess = await GetMessageByIdAsync(id);
            if (mess == null)
                return NotFound();
            await SetViewData();
            return View(mess);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") == "Admin" || HttpContext.Session.GetString("r") == null)
            {
                return RedirectToAction("Index");
            }
            var accountId = session.GetString("AccountId");
            if (accountId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var message = await GetMessageByIdAsync(id);

            if (accountId != message.AccountId.ToString())
            {
                return RedirectToAction("Index", "Home");
            }

            HttpResponseMessage response = await client.DeleteAsync(MessageApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }







        private async Task<Message> GetMessageByIdAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync(MessageApiUrl + "/" + id);
            if (!response.IsSuccessStatusCode)
                return null;
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<Message>(strData, options);
        }




        public async Task SetViewData()
        {
            var listAccount = await GetAccountAsync();
            ViewData["Accounts"] = new SelectList(listAccount, "AccountId", "Fullname");
        }


        public async Task<IEnumerable<Account>> GetAccountAsync()
        {
            HttpResponseMessage response = await client.GetAsync(AccountApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<Account> listAccounts = JsonSerializer.Deserialize<List<Account>>(strData, options);
            return listAccounts;
        }



        
        public async Task<ActionResult> AdminDelete(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var mess = await GetMessageByIdAsync(id);
            if (mess == null)
                return NotFound();
            await SetViewData();
            return View(mess);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("r") != "Admin")
            {
                return RedirectToAction("Index");
            }

            var message = await GetMessageByIdAsync(id);

         
            HttpResponseMessage response = await client.DeleteAsync(MessageApiUrl + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
