using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SharedModels
{
    public class ArenaResult
    {
        public string DuelName { get; set; }
        public string DuelWinner { get; set; }
        public string DuelLoser { get; set; }
    }
    public partial class ArenaResultMessage
    {
        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("end")]
        public bool End { get; set; }
        [JsonProperty("duelWinner")]
        public string DuelWinner { get; set; }
        [JsonProperty("duelLoser")]
        public string DuelLoser { get; set; }
    }

    public partial class ArenaResultMessage
    {
        public static ArenaResultMessage FromJson(string json) => JsonConvert.DeserializeObject<ArenaResultMessage>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this ArenaResultMessage self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
