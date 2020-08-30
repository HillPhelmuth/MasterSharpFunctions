using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CompileFunction.Models;
using CompileFunction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CompileFunction.Functions
{
    public class CompileCode
    {
        private readonly CompilerService _compilerService;
        public CompileCode(CompilerService compilerService)
        {
            _compilerService = compilerService;
        }

        [FunctionName("CompileCode")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "code")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP/code trigger function processed a request.");

            var executableReferences = CompileResources.PortableExecutableReferences;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var codeInput = JsonConvert.DeserializeObject<CodeInputModel>(requestBody);
            var testCode = codeInput.Solution;
            var result = await _compilerService.SubmitCode(testCode, executableReferences);
            
            return new OkObjectResult(result);
        }


    }
}
