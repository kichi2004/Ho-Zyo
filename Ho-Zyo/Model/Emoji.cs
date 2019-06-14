using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace Ho_Zyo.Model
{
    public partial class Emoji
    {
        [JsonProperty("managed")]
        public bool Managed { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("roles")]
        public List<object> Roles { get; set; }

        [JsonProperty("require_colons")]
        public bool RequireColons { get; set; }

        [JsonProperty("animated")]
        public bool Animated { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return $"<:{Name}:{Id}>";
        }

        public static IEnumerable<Emoji> GetEmojis()
        {
            var client = new RestClient("https://discordapp.com/api");
            const string serverId = "522072012932251648";
            var request = new RestRequest($"/guilds/{serverId}/emojis", Method.GET);

            var token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            request.AddHeader("Authorization", "Bot " + token);
            request.AddHeader("User-Agent", "DiscordBot");
            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute<List<Emoji>>(request);
            return response.Data;
        }
    }

    public partial class Emoji
    {
        public static List<Emoji> FromJson(string json)
            => JsonConvert.DeserializeObject<List<Emoji>>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<Emoji> self)
            => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling =
                MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter
                {
                    DateTimeStyles =
                        DateTimeStyles.AssumeUniversal
                }
            }
        };
    }
}