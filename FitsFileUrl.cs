using Newtonsoft.Json;

public class FitsFileUrl
{
    [JsonProperty("record")]
    public string Record { get; set; } = null!;
    [JsonProperty("filename")]
    public string Filename { get; set; } = null!;
}