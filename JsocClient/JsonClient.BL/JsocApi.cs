using System.Net.Http.Json;
using Newtonsoft.Json.Converters;

namespace JsonClient.BL
{

    public class JsocApi
    {
        private HttpClient _httpClient;
        private string _notify;
        public JsocApi()
        {
            _notify = "kobzarev.dani@gmail.com";
            _httpClient = new HttpClient();
        }

        public async Task<bool> IsExistingUncompletedRequest()
        {
            string endpoint = $"http://jsoc.stanford.edu/cgi-bin/ajax/manage-request.sh?address={_notify}&operation=check&H=hmidb2";
            var request = await _httpClient.GetAsync(endpoint);
            var response = await request.Content.ReadFromJsonAsync<SubmitResponse>();
            return response.Status == 1;
        }

        public async Task WaitForCurrentRequestToComplete()
        {
            bool isComplete = await IsExistingUncompletedRequest();
            while (isComplete == false)
            {
                await Task.Delay(1000);
                isComplete = await IsExistingUncompletedRequest();
                Console.WriteLine("Жду окончание обработки запроса...");
            }
        }

        public async Task Cancel()
        {
            string endpoint = $"http://jsoc.stanford.edu/cgi-bin/ajax/manage-request.sh?address={_notify}&operation=cancel&H=hmidb2";
            HttpResponseMessage request = await _httpClient.GetAsync(endpoint);
            var response = await request.Content.ReadFromJsonAsync<CancelResponse>();
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
            var result = await response.Content.ReadFromJsonAsync<SubmitResponse>();
            if (result.Status > 2)
                throw new UnknowException(result.Status);

            string requestId = result.RequestId;
            return requestId;
        }

        public async Task<LinksToFitsFiles> GetLinksToFitsFiles(string requestId)
        {
            string endpoint = $"http://jsoc.stanford.edu/cgi-bin/ajax/jsoc_fetch?op=exp_status&requestid={requestId}";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            var files = await response.Content.ReadFromJsonAsync<LinksToFitsFiles>();
            return files;
        }
    }
}
