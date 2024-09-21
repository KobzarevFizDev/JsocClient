using Newtonsoft.Json;

namespace JsonClient.BL
{
    public class FitsFileUrl
    {
        [JsonProperty("record")]
        public string Record { get; set; } = null!;
        [JsonProperty("filename")]
        public string Filename { get; set; } = null!;
    }
}
