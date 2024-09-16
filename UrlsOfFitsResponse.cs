using Newtonsoft.Json;

public class UrlsOfFitsResponse
{
    [JsonProperty("version")]
    public string Version { get; set; } = null!;
    [JsonProperty("requestid")]
    public string RequestId { get; set; } = null!;
    [JsonProperty("method")]
    public string Method { get; set; } = null!;
    [JsonProperty("protocol")]
    public string Protocol { get; set; } = null!;
    [JsonProperty("count")]
    public int Count { get; set; }
    [JsonProperty("size")]
    public int Size { get; set; }
    [JsonProperty("exptime")]
    public string Exptime { get; set; } = null!;
    [JsonProperty("dir")]
    public string Dir { get; set; } = null!;
    [JsonProperty("status")]
    public int Status { get; set; }
    [JsonProperty("keywords")]
    public string Keywords { get; set; } = null!;
    [JsonProperty("data")]
    public FitsFileUrl[] Data { get; set; } = null!;
}