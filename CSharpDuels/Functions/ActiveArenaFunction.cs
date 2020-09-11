using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CSharpDuels.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedModels;

namespace CSharpDuels.Functions
{
    public class ActiveArenaFunction
    {
        private readonly CSharpDuelsDbContext _context;

        public ActiveArenaFunction(CSharpDuelsDbContext context)
        {
            _context = context;
        }

        [FunctionName("GetActiveArenas")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getAllArenas")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var allArenas = await _context.ActiveArenas.ToListAsync();

            return new OkObjectResult(allArenas);
        }
        [FunctionName("AddActiveArenas")]
        public async Task<IActionResult> AddArena(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "addArena")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var arena = JsonConvert.DeserializeObject<Arena>(requestBody);
            log.LogInformation($"Arena Created: {arena.Name}");
            await _context.ActiveArenas.AddAsync(arena);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        [FunctionName("UpdateActiveArenas")]
        public async Task<IActionResult> UpdateArena(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "joinArena/{arenaId}/{userName}")] HttpRequest req,
            string arenaId, string userName, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            
            var arenas = await _context.ActiveArenas.Where(x => x.Id == arenaId).ToListAsync();
            var arena = arenas.FirstOrDefault();
            if (arena == null)
                return new BadRequestErrorMessageResult($"No Active Arena with id {arenaId} exists in database");
            arena.Opponent = userName;
            _context.ActiveArenas.Update(arena);
            await _context.SaveChangesAsync();
            log.LogInformation($"Arena Joined: {arena.Name} by {userName}");
            return new OkResult();
        }
        [FunctionName("RemoveActiveArenas")]
        public async Task<IActionResult> RemoveArena(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "removeArena/{arenaId}")] HttpRequest req,
            string arenaId, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var arenas = await _context.ActiveArenas.Where(x => x.Id == arenaId).ToListAsync();
            var arena = arenas.FirstOrDefault();
            if (arena == null)
                return new BadRequestErrorMessageResult($"No Active Arena with id {arenaId} exists in database");
           
            _context.ActiveArenas.Remove(arena);
            await _context.SaveChangesAsync();
            log.LogInformation($"Arena Remove: {arena.Name}");
            return new OkResult();
        }

    }
}
