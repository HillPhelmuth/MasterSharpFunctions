using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSharpDuels.DataContext;
using CSharpDuels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSharpDuels.Functions
{
    public class DuelHttpBound
    {
        private readonly CSharpDuelsDbContext _context;

        public DuelHttpBound(CSharpDuelsDbContext context)
        {
            _context = context;
        }

        [FunctionName("DuelEntity")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "addUser")] object duelModel,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var jsonString = duelModel.ToString();
            var data = DuelModel.FromJson(jsonString); 
            //if(data.Id == null)
            //    data.Id = new Guid();
            await _context.Duels.AddAsync(data);
            await _context.SaveChangesAsync();
            
            var jsonSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            string responseMessage = data.ToJson();
            log.LogInformation(responseMessage);
            return new OkResult();
        }

        [FunctionName("DuelUpdate")]
        public async Task<IActionResult> Runonce(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "updateUser")]
            DuelModel duelModel, ILogger log)
        {
            var matchedUser = await _context.Duels.FirstOrDefaultAsync(x => x.UserId == duelModel.UserId);
            matchedUser.CompletedDuels = new List<CompletedDuel>();
            foreach (var completedDuel in duelModel.CompletedDuels)
            {
                matchedUser.CompletedDuels.Add(completedDuel);
            }

            _context.Duels.Update(matchedUser);
            var result = await _context.SaveChangesAsync();
            
            return new OkObjectResult(result);
        }
        //[FunctionName("DuelEntityFirst")]
        //public async Task<IActionResult> Runonce(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "First")] HttpRequest req,
        //    ILogger log)
        //{
        //    log.LogInformation("C# HTTP trigger function processed a request.");


        //    DuelModel initial = new DuelModel(){UserId = "HillPhel", CompletedDuels = new List<CompletedDuel>()};
        //    var completed = new CompletedDuel()
        //    {
        //        Attempts = 1,
        //        ChallengeName = "Braces",
        //        Duel = initial,
        //        DuelId = "1|GoTime",
        //        RivalId = "adam@adam",
        //        Solution = "var x = 1; var y = 2; return x+y;",
        //        Time = new TimeSpan(0,10,30),
        //        WonDuel = true
        //    };
        //    initial.CompletedDuels.Add(completed);

        //    //log.LogInformation($"The stringified List Property: {initial.DuelsWonAsJson}");
        //    //await _context.Database.EnsureCreatedAsync();
        //    await _context.Duels.AddAsync(initial);
        //    await _context.SaveChangesAsync();
        //    var jsonSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        //    string responseMessage = JsonConvert.SerializeObject(initial, Formatting.Indented, jsonSettings);
        //    log.LogInformation($"JSON: {initial}");
        //    return new OkObjectResult(responseMessage);
        //}
    }
}
