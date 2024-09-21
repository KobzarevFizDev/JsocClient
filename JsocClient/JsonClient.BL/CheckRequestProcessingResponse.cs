using Newtonsoft.Json;


namespace JsonClient.BL
{
    internal class CheckRequestProcessingResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; } = null!;
    }
}
