using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CSharpDuels.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CSharpDuels.Functions
{
    public static class DuelCosmosBound
    {
        
        private static readonly string authKey = Environment.GetEnvironmentVariable("AuthKey");
        private static readonly string endpoint = Environment.GetEnvironmentVariable("ServiceEndpoint");
        private static readonly string dbName = Environment.GetEnvironmentVariable("CSharpDuelsDb");
        private static readonly string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        private static CosmosClient cosmosClient = new CosmosClient(endpoint, authKey);
        private static IDocumentClient docClient = new DocumentClient(new Uri(endpoint), authKey);
        private const string FunctionBaseUrl = "https://csharpduelshubfunction.azurewebsites.net/api";
       
        [FunctionName("DuelCosmosBound")]
        public static async void Run([CosmosDBTrigger(
                databaseName: "CSharpDuelsDb",
                collectionName: "DuelsData",
                ConnectionStringSetting = "ConnectionString",
                LeaseCollectionName = "leases",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input, ILogger log)
        {
            var sw = new Stopwatch();

            if (input == null || input.Count <= 0) return;
            sw.Start();
            var document = input.FirstOrDefault();
            var userId = document?.GetPropertyValue<string>("userId");
            var completedDuels = document?.GetPropertyValue<List<CompletedDuel>>("completedDuels");
            var completedDuel = completedDuels?.OrderByDescending(x => x.TimeCompleted).FirstOrDefault();
            string output = "";
            var userWon = completedDuel?.WonDuel ?? false;
                
            output = userWon ? $"{userId} Defeated {completedDuel?.RivalId} in challnge {completedDuel?.ChallengeName}!" : $"{completedDuel?.RivalId} Defeated {userId} in challnge {completedDuel?.ChallengeName}!";
            sw.Stop();

            log.LogInformation("Documents modified " + input.Count);
            log.LogInformation("First document Id " + input[0].Id);
            log.LogInformation(output);
            var client = new HttpClient();
            var url = $"{FunctionBaseUrl}/alerts/CosmosDb";
            var message = $"{output}";
            await client.PostAsJsonAsync(url, message);
        }
    }
}
