using System.Collections.Generic;
using Newtonsoft.Json;
using SharedModels;

namespace ChallengeFunction.Models
{
    
    public class CodeChallenges
    {
        [JsonProperty("challenges")]
        public List<Challenge> Challenges { get; set; }
    }

    

}
