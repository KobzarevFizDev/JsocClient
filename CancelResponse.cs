using Newtonsoft.Json;

public class CancelResponse
{
    [JsonProperty("status")]
    public int Status { get; set; }

    [JsonProperty("msg")]
    public string Message { get; set; } = null!;
}