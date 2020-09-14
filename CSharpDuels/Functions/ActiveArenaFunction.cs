using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
//using CSharpDuels.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedModels;

namespace CSharpDuels.Functions
{
    public class ActiveArenaFunction
    {
        //private readonly CSharpDuelsDbContext _context;
        private Container _database;
        public ActiveArenaFunction(CosmosClient client)
        {
            //_context = context;
            _database = client.GetContainer("ActiveArenasDb", "ActiveArenas");
        }

        [FunctionName("GetActiveArenas")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getAllArenas")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for GetActiveArenas.");

            //var allArenas = await _context.ActiveArenas.ToListAsync();
            var arenasIterator = _database.GetItemLinqQueryable<Arena>().ToFeedIterator();
            var arenaList = new List<Arena>();
            while (arenasIterator.HasMoreResults)
            {
                var resultSet = await arenasIterator.ReadNextAsync();
                arenaList.AddRange(resultSet);
            }
            return new OkObjectResult(arenaList);
        }
        [FunctionName("AddActiveArenas")]
        public async Task<IActionResult> AddArena(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "addArena")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for AddActiveArenas.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var arena = JsonConvert.DeserializeObject<Arena>(requestBody);
            log.LogInformation($"Arena Created: {requestBody}");
            await _database.CreateItemAsync(arena);

            //await _context.ActiveArenas.AddAsync(arena);
            //await _context.SaveChangesAsync();
            return new OkResult();
        }
        [FunctionName("UpdateActiveArenas")]
        public async Task<IActionResult> UpdateArena(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "joinArena/{arenaId}/{name}")] HttpRequest req,
            string arenaId, string name, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for UpdateActiveArenas.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var arena = JsonConvert.DeserializeObject<Arena>(requestBody);
            var partitionKey = new PartitionKey(name);
            var response = await _database.UpsertItemAsync(arena);
            log.LogInformation($"Upsert Status Code: {response.StatusCode} For Arena: {response.Resource?.Name}");
            //var arenas = await _context.ActiveArenas.Where(x => x.Id == arenaId).ToListAsync();
            //var arena = arenas.FirstOrDefault();
            //if (arena == null)
            //    return new BadRequestErrorMessageResult($"No Active Arena with id {arenaId} exists in database");
            //arena.Opponent = userName;
            //_context.ActiveArenas.Update(arena);
            //log.LogInformation($"Arena Join attempt: Id: {arena.Id} Name: {arena.Name} by {userName}");
            //await _context.SaveChangesAsync();
            return new OkResult();
        }
        [FunctionName("RemoveActiveArenas")]
        public async Task<IActionResult> RemoveArena(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "removeArena/{arenaId}/{name}")] HttpRequest req,
            string arenaId, string name, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request for RemoveActiveArenas.");

            //var arenas = await _context.ActiveArenas.Where(x => x.Id == arenaId).ToListAsync();
            //var arena = arenas.FirstOrDefault();
            var response = await _database.DeleteItemAsync<Arena>(arenaId, new PartitionKey(name));
            //if (arena == null)
            //    return new BadRequestErrorMessageResult($"No Active Arena with id {arenaId} exists in database");

            //_context.ActiveArenas.Remove(arena);
            //await _context.SaveChangesAsync();
            log.LogInformation($"Arena Remove: {response.StatusCode} for Arena: {response.Resource?.Name}");
            return new OkResult();
        }

    }
}
