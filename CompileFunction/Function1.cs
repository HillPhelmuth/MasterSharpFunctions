using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CompileFunction
{
    public static class Function1
    {

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var appAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<PortableExecutableReference> _myRefs = new List<PortableExecutableReference>();
            var narrowedRefs = appAssemblies.Where(x =>
                !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location) &&
                (x.GetName().Name == "System" || x.GetName().Name == "System.Core" || x.GetName().Name == "System.Numerics" || x.GetName().Name == "mscorlib" /*|| x.GetName().Name == "netstandard"*/));
            
            foreach (var nref in narrowedRefs)
            {
                var metaData = MetadataReference.CreateFromFile(nref.Location);
                _myRefs.Add(metaData);
            }


            log.LogInformation("C# HTTP trigger function processed a request.");

            //string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(data);
        }
    }
}
