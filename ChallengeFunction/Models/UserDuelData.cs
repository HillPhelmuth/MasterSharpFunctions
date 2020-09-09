using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ChallengeFunction.Models
{
    public class UserDuelData
    {
    }
    public class UserDuel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("completedDuels")]
        public virtual List<ArenaDuel> CompletedDuelsList { get; set; }

    }
}
