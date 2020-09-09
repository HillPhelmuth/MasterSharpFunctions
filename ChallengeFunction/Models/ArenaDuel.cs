using System;
using Newtonsoft.Json;

namespace ChallengeFunction.Models
{
    public class ArenaDuel
    {
        public int ID { get; set; }
        public int UserAppDataID { get; set; }
        [JsonProperty("duelId")]
        public string DuelName { get; set; }

        [JsonProperty("challengeName")]
        public string ChallengeName { get; set; }

        [JsonProperty("rivalId")]
        public string RivalId { get; set; }

        [JsonProperty("solution")]
        public string Solution { get; set; }

        [JsonProperty("timeCompleted")]
        public DateTime TimeCompleted { get; set; }

        [JsonProperty("wonDuel")]
        public bool WonDuel { get; set; }

        public override string ToString()
        {
            return $"{DuelName},{ChallengeName},{RivalId},{Solution},{WonDuel},{TimeCompleted}";
        }
    }
}