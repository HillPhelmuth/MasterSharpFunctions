using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ChallengeFunction.Data;
using ChallengeFunction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ChallengeFunction.Functions
{
    public class UserDbFunction
    {
        private readonly ChallengeContext context;

        public UserDbFunction(ChallengeContext context)
        {
            this.context = context;
        }

        [FunctionName("GetUserData")]
        public async Task<IActionResult> GetUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "users/{userName}")] HttpRequest req,
            ILogger log, string userName)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var users = await context.UserAppData.ToListAsync();
            foreach (var user in users)
            {
                log.LogInformation($"{user.Name} found");
            }
            if (users.Any(x => x.Name == userName))
            {
                var currentUser = users.FirstOrDefault(x => x.Name == userName);
                var userSnippets = await context.UserSnippets.ToListAsync();
                currentUser.Snippets = userSnippets.Where(x => x.UserAppDataID == currentUser.ID).ToList();
                JsonSerializerSettings settings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                var logString = JsonConvert.SerializeObject(currentUser, Formatting.Indented, settings);
                log.LogInformation(logString);
                return new OkObjectResult(currentUser);
            }
            
            await context.UserAppData.AddAsync(new UserAppData { Name = userName });
            await context.SaveChangesAsync();
            return new OkObjectResult(new UserAppData { Name = userName });
        }

        
        [FunctionName("AddUserSnippet")]
        public async Task<IActionResult> AddSnippet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "addSnippet/{userName}")]
            HttpRequest req, string userName,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var snippet = JsonConvert.DeserializeObject<UserSnippet>(requestBody);
            if (string.IsNullOrEmpty(userName))
                return new BadRequestErrorMessageResult("Parameter UserName is required");
            if (!await context.UserAppData.AnyAsync(x => x.Name == userName))
                return new BadRequestErrorMessageResult("User does not exist in database");
            var currentUser = await context.UserAppData.FirstOrDefaultAsync(x => x.Name == userName);
            snippet.UserAppDataID = currentUser.ID;
            await context.UserSnippets.AddAsync(snippet);
            await context.SaveChangesAsync();
            return new OkResult();

        }

        [FunctionName("UpdateUserChallenge")]
        public async Task<IActionResult> UpdateUserChallenge(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "addSnippet/{userName}/{challengeId}")]
            HttpRequest req, int challengeId, string userName,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request to updateUserChallenge (challengeId: {challengeId})");
            if (string.IsNullOrEmpty(userName))
                return new BadRequestErrorMessageResult("Parameter UserName is required");
            if (!await context.UserAppData.AnyAsync(x => x.Name == userName))
                return new BadRequestErrorMessageResult("User does not exist in database");
            var currentUser = await context.UserAppData.FirstOrDefaultAsync(x => x.Name == userName);
            var currentChallengeString = currentUser.ChallengeSuccessData ?? "";
            currentUser.ChallengeSuccessData = $"{currentChallengeString},{challengeId}";
            await context.SaveChangesAsync();
            return new OkResult();
        }
    }
}