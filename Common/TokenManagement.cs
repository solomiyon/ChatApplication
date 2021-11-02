using Newtonsoft.Json;
using System;

namespace Common
{
    [JsonObject("tokenManagement")]
    public class TokenManagement
    {
        [JsonProperty("JwtKey")]
        public string JwtKey { get; set; }

        [JsonProperty("JwtIssuer")]
        public string JwtIssuer { get; set; }

        [JsonProperty("JwtExpireDays")]
        public int JwtExpireDays { get; set; }
    }
}
