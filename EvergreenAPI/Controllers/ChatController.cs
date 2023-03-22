using EvergreenAPI.Models;
using EvergreenAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Completions;
using OpenAI_API;
using System.Threading.Tasks;
using System.Linq;

namespace EvergreenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private AppDbContext _context;
        private string ApiKey = "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";

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public IActionResult GetMessages(int userId)
        {
            var account = _context.Accounts.Where(a => a.AccountId == userId).FirstOrDefault();
            if (account == null) return BadRequest("Account ID does not exist");
            return Ok(account.Chat);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> GetResponse(int userId, [FromBody] string prompt)
        {
            var account = _context.Accounts.Where(a => a.AccountId == userId).FirstOrDefault();
            if (account == null) return BadRequest("Account ID does not exist");

            string answer = string.Empty;
            var openai = new OpenAIAPI(ApiKey);
            var completion = new CompletionRequest();

            // Load old conversation data
            completion.Prompt = account.Chat + "\\nHuman: " + prompt;

            completion.MaxTokens = 1000;
            var result = openai.Completions.CreateCompletionAsync(completion);
            if (result != null)
            {
                foreach (var item in result.Result.Completions)
                {
                    answer = item.Text;
                }
                
                // Remove new line at the start of answer
                while (true)
                {
                    if (answer.StartsWith("\\n")) answer = answer.Substring(1);
                    else break;
                }

                // Remove the "AI: " at the start of answer
                while (true)
                {
                    var index = answer.IndexOf("AI: ");
                    if (index != -1)
                        answer = answer.Substring(index + 3);
                    else break;
                }

                // Save answer to history
                answer = answer.Trim();
                account.Chat = account.Chat + "/eol/Human: " + prompt + "/eol/AI: " + answer;
                await _context.SaveChangesAsync();

                return Ok(answer);
            }
            else
            {
                return BadRequest("Not found");
            }
        }
    }
}
