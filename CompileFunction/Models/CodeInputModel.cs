using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;

namespace CompileFunction.Models
{
    public class CodeInputModel
    {
        [JsonProperty("solution")]
        public string Solution { get; set; }
        [JsonProperty("tests")]
        public List<Test> Tests { get; set; }
    }

    public class Test
    {
        [JsonProperty("append")]
        public string Append { get; set; }
        [JsonProperty("testAgainst")]
        public string TestAgainst { get; set; }
    }
}
