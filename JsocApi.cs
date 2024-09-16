using System.Net.Http.Json;
using Newtonsoft.Json.Converters;

public class JsocApi
{
    private HttpClient _httpClient;
    private string _notify;
    public JsocApi()
    {
        _notify = "kobzarev.dani@gmail.com";
        _httpClient = new HttpClient();
    }

    public async Task Check()
    {
        string endpoint = $"http://jsoc.stanford.edu/cgi-bin/ajax/manage-request.sh?address={_notify}&operation=check&H=hmidb2";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        var r = await response.Content.ReadAsStringAsync();
        Console.WriteLine("JsocApi::CheckParametrs(). Response = " + r);
    }


    public async Task<string> Submit(DateTime fromTime, int durationInMinutes, int channel)
    {
        int year = fromTime.Year;
        int month = fromTime.Month;
        int day = fromTime.Day;
        string endpoint = "http://jsoc.stanford.edu/cgi-bin/ajax/jsocextfetch";

        var parametrs = new Dictionary<string, string>();
        parametrs.Add("op", "exp_request");
        parametrs.Add("ds", $"aia.lev1_euv_12s[{year}.{month}.{day}_12:40/{durationInMinutes}m][? WAVELNTH = {channel} ?]");
        parametrs.Add("sizeratio", "1");
        parametrs.Add("process", "n=0|no_op");
        parametrs.Add("notify", _notify);
        parametrs.Add("method", "url");
        parametrs.Add("filenamefmt", "aia.lev1_euv_12s.{T_REC:A}.{WAVELNTH}.{segment}");
        parametrs.Add("format", "json");
        parametrs.Add("protocol", "FITS,compress Rice");
        parametrs.Add("dbhost", "hmidb2");

        HttpContent form = new FormUrlEncodedContent(parametrs);


        HttpResponseMessage response = await _httpClient.PostAsync(endpoint, form);
        var result = await response.Content.ReadFromJsonAsync<FetchResult>();
        if (result.Status > 2)
            throw new ProcessingExportRequestException(result.Status);

        string requestId = result.RequestId;
        return requestId;
    }

    public async Task<UrlsOfFitsResponse> GetUrlsOfFiles(string requestId)
    {
        string endpoint = $"http://jsoc.stanford.edu/cgi-bin/ajax/jsoc_fetch?op=exp_status&requestid={requestId}";
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        var files = await response.Content.ReadFromJsonAsync<UrlsOfFitsResponse>();
        return files;
    }
}