using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSharpDuels.Models
{
    public class UserDuel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("completedDuels")]
        public List<CompletedDuel> CompletedDuelsList { get; set; }
        //public static UserDuel FromJson(string json) => JsonConvert.DeserializeObject<UserDuel>(json, Converter.Settings);

    }
    public partial class Completed
    {
        [JsonProperty("completedDuels")]
        public List<CompletedDuel> CompletedDuels { get; set; }
    }
    public class CompletedDuel
    {
        [JsonProperty("duelId")]
        public string DuelId { get; set; }

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
            return $"{DuelId},{ChallengeName},{RivalId},{Solution},{WonDuel},{TimeCompleted}";
        }
    }
    //public partial class Completed
    //{
    //    public static Completed FromJson(string json) => JsonConvert.DeserializeObject<Completed>(json, Converter.Settings);
    //}

    //public static class Serialize
    //{
    //    public static string ToJson(this Completed self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //}

    //public static class SerializeWhole
    //{
    //    public static string ToJson(this UserDuel self) => JsonConvert.SerializeObject(self, Converter.Settings);
    //}
    //var settings=new JsonSerializerSettings{DateFormatString ="yyyy-MM-ddTHH:mm"};
    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        DateFormatString = "yyyy-MM-ddTHH:mmK",
    //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.DateTime,
    //        Converters =
    //        {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}
}
