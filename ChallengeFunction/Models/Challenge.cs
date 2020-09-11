using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChallengeFunction.Models
{
    public class Challenge
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("difficulty")]
        public string Difficulty { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("examples")]
        public string Examples { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }
        [JsonProperty("solution")]
        public string Solution { get; set; }
        [JsonProperty("tests")]
        public virtual List<Test> Tests { get; set; }
        
        public string AddedBy { get; set; }
        
    }
}