using Newtonsoft.Json;
using System;

[Serializable]
public class FetchResult
{
    [JsonProperty("status")]
    public int Status { get; set; }
    [JsonProperty("requestid")]
    public string RequestId { get; set; } = null!;
    [JsonProperty("method")]
    public string Method { get; set; } = null!;
    [JsonProperty("protocol")]
    public string Protocol { get; set; } = null!;
    [JsonProperty("wait")]
    public int Wait { get; set; }
    [JsonProperty("rcount")]
    public int Rcount { get; set; }
    [JsonProperty("size")]
    public int Size { get; set; }
}