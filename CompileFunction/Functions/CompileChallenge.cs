using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CompileFunction.Models;
using CompileFunction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CompileFunction.Functions
{
    public class CompileChallenge
    {
        private readonly CompilerService _compilerService;
        public CompileChallenge(CompilerService compilerService)
        {
            _compilerService = compilerService;
        }
        [FunctionName("CompileChallenge")]
        public async Task<IActionResult> RunSubmitChallenge(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "challenge")] HttpRequest req,
            ILogger log)
        {
            var executableReferences = CompileResources.PortableExecutableReferences;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var codeInput = JsonConvert.DeserializeObject<CodeInputModel>(requestBody);
            var testCode = codeInput.Solution;
            var outputs = new CodeOutputModel { Outputs = new List<Output>() };
            int index = 1;
            if (testCode == "TestPing")
            {
                _ = _compilerService.SubmitCode("return -1;", executableReferences).Result;
                return new OkObjectResult("PingBack");
            }
            var swTot = new Stopwatch();
            log.LogInformation("start all tasks");
            swTot.Start();
            await Task.Run(() =>
            {
                foreach (var snip in codeInput.Tests)
                {
                    var sw = new Stopwatch();

                    var output = new Output { TestIndex = index, Test = snip };
                    string code = $"{testCode}\n{snip.Append}";
                    string expected = snip.TestAgainst;

                    log.LogInformation("Start task 1");
                    sw.Start();

                    output.Codeout = _compilerService.SubmitCode(code, executableReferences).Result;
                    sw.Stop();
                    log.LogInformation($"Complete task 1 in {sw.ElapsedMilliseconds}ms");
                    log.LogInformation("\nStart task 2");
                    sw.Restart();
                    output.TestResult = _compilerService.SubmitSolution(code, executableReferences, expected).Result;
                    sw.Stop();
                    log.LogInformation($"Complete task 2 in {sw.ElapsedMilliseconds}ms");
                    index++;
                    outputs.Outputs.Add(output);

                }
            });
            swTot.Stop();
            log.LogInformation($"Completed all tasks in {swTot.ElapsedMilliseconds}ms");
            return new OkObjectResult(outputs);

        }

    }

}
