using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CSharpDuels.DataContext;
using CSharpDuels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSharpDuels.Functions
{
    public class DuelHttpBound
    {
        private readonly CSharpDuelsDbContext _context;

        public DuelHttpBound(CSharpDuelsDbContext context)
        {
            _context = context;
        }

        [FunctionName("AddUpdateDuel")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "addUpdate/{userId}")] CompletedDuel completed, string userId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var userDuel = await _context.Duels.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (userDuel != null)
            {
                if (userDuel.CompletedDuelsList != null)
                {
                    userDuel.CompletedDuelsList.Add(completed);
                }
                else
                {
                    userDuel.CompletedDuelsList = new List<CompletedDuel> {completed};
                }

                _context.Duels.Update(userDuel);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            var newUserDuel = new UserDuel()
            {
                UserId = userId,
                CompletedDuelsList = new List<CompletedDuel>{completed}
            };
            
            
            await _context.Duels.AddAsync(newUserDuel);
            await _context.SaveChangesAsync();

            string responseMessage = completed.ToString();
            log.LogInformation(responseMessage);
            return new OkResult();
        }

        [FunctionName("GetDuels")]
        public async Task<IActionResult> RunGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Get/{userId}")]
            object duelModel, string userId, ILogger log)
        {
            var userDuel = await _context.Duels.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (userDuel == null)
            {
                return new BadRequestErrorMessageResult("User does not exist in Duels document");
            }
            return new OkObjectResult(userDuel);
        }
        
    }
}
