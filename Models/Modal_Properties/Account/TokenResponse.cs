using Newtonsoft.Json;

namespace VigilanceClearance.Models.Modal_Properties.Account
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }

}
