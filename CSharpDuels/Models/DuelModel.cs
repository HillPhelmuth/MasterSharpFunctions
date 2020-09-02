using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSharpDuels.Models
{
    public partial class DuelModel
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public int Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("duelsWonAsJson")]
        public string DuelsWonAsJson { get; set; }

        [JsonProperty("completedDuels")]
        [NotMapped]
        public Completed CompletedDuels { get; set; }

       
    }


    //public partial class Completed
    //{
    //    [JsonProperty("completedDuels")]
    //    public List<CompletedDuel> CompletedDuels { get; set; }
    //}

    //public partial class CompletedDuel
    //{
    //    [JsonProperty("duelId")]
    //    public string DuelId { get; set; }

    //    [JsonProperty("challengeName")]
    //    public string ChallengeName { get; set; }

    //    [JsonProperty("rivalId")]
    //    public string RivalId { get; set; }

    //    [JsonProperty("solution")]
    //    public string Solution { get; set; }

    //    [JsonProperty("timeCompleted")]
    //    public DateTime TimeCompleted { get; set; }

    //    [JsonProperty("wonDuel")]
    //    public bool WonDuel { get; set; }
    //}

    public partial class Completedx
    {
        public static Completed FromJson(string json) => JsonConvert.DeserializeObject<Completed>(json, Converter.Settings);
    }

    public static class Serializex
    {
        public static string ToJson(this DuelModel self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }
    //var settings=new JsonSerializerSettings{DateFormatString ="yyyy-MM-ddTHH:mm"};
    internal static class Converterx
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-ddTHH:mmK",
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.DateTime,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(int) || t == typeof(int?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            int l;
            if (int.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type int");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (int)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

    }

}