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
        private static readonly string _containerId = "collection2";
        private static readonly string authKey = Environment.GetEnvironmentVariable("AuthKey");
        private static readonly string endpoint = Environment.GetEnvironmentVariable("ServiceEndpoint");
        private static readonly string dbName = Environment.GetEnvironmentVariable("DatabaseName");
        private static readonly string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        private static CosmosClient cosmosClient = new CosmosClient(endpoint, authKey);
        private static IDocumentClient docClient = new DocumentClient(new Uri(endpoint), authKey);
        private const string FunctionBaseUrl = "https://csharprealtimefunction.azurewebsites.net/api";

        [FunctionName("DuelCosmosBound")]
        public static async void Run([CosmosDBTrigger(
                databaseName: "CSharpDuelsDb",
                collectionName: "UserDuels",
                ConnectionStringSetting = "ConnectionString",
                LeaseCollectionName = "leases",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input, ILogger log)
        {
            var sw = new Stopwatch();
            
            if (input != null && input.Count > 0)
            {
                sw.Start();
                string output = "";
                var queryDefinition = new QueryDefinition("select * From UserDuels");
                //var feedIterator = cosmosClient.GetDatabaseQueryStreamIterator(queryDefinition);
                using var feedIterator = cosmosClient.GetDatabaseQueryStreamIterator(
                    queryDefinition);
                var sb = new StringBuilder(" ");
                while (feedIterator.HasMoreResults)
                {
                    // Stream iterator returns a response with status for errors
                    using var response = await feedIterator.ReadNextAsync();
                    // Handle failure scenario. 
                    if (!response.IsSuccessStatusCode)
                    {
                        
                        // Log the response.Diagnostics and handle the error
                    }
                    var reader = new StreamReader(response.Content);
                    var outp = await reader.ReadToEndAsync();
                    sb.Append(outp);
                }

                output = sb.ToString();
                sw.Stop();

                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
                log.LogInformation(output);
                var client = new HttpClient();
                var url = $"{FunctionBaseUrl}/messages/AUTOMATED";
                var message = $"from Cosmos: took {sw.ElapsedMilliseconds}ms User {output} ";
                await client.PostAsJsonAsync(url, message);
            }
        }
    }
}
