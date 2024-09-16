using Newtonsoft.Json;

public class CheckExistingRequest
{
    [JsonProperty("msg")]
    public string Message { get; set; } = null!;
    [JsonProperty("status")]
    public int Status { get; set; }
}