using Newtonsoft.Json;


namespace JsonClient.BL
{
    public class CheckExistingRequest
    {
        [JsonProperty("msg")]
        public string Message { get; set; } = null!;
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
