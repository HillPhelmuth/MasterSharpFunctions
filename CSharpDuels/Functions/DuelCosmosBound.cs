using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using CSharpDuels.Models;
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
        
        private const string FunctionBaseUrl = "https://csharpduelshubfunction.azurewebsites.net/api";
       
        [FunctionName("DuelCosmosBound")]
        public static async void Run([CosmosDBTrigger(
                databaseName: "ActiveArenasDb",
                collectionName: "ActiveArenas",
                ConnectionStringSetting = "ConnectionString",
                LeaseCollectionName = "leases",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input, ILogger log)
        {
           
            if (input == null || input.Count <= 0) return;
            log.LogInformation("Documents modified " + input.Count);
            log.LogInformation("First document Id " + input[0].Id);
            log.LogInformation("Tracking arenas:");
            foreach (var doc in input)
            {
                log.LogInformation($"Arena: {doc}");
            }
           
          
            
            var client = new HttpClient();
            var url = $"{FunctionBaseUrl}/alert/";
            var message = $"{input[0].Id}";
            await client.PostAsJsonAsync(url, message);
        }
    }
}
