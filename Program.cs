Console.WriteLine("Welcome to JSOC Parser!");

var jsocApi = new JsocApi();

var fromTime = new DateTime(2018, 10, 27);
int duration = 80;
int channel = 94;

Console.WriteLine("Checking existing requests...");
bool existingUncompletedRequest = await jsocApi.IsExistingUncompletedRequest();
if (existingUncompletedRequest)
{
    Console.WriteLine("Existing requests found!");
    Console.WriteLine("Canceling...");
    await jsocApi.Cancel();
}

try
{
    Console.WriteLine("Not found existing request");
    Console.WriteLine("Creating request...");
    string requestId = await jsocApi.Submit(fromTime, duration, channel);
    Console.WriteLine($"Request created. RequestId = {requestId}");
    // этот запрос только создает заявку, он не возращает url
    await jsocApi.WaitForCurrentRequestToComplete();
    var urlsOfFits = await jsocApi.GetUrlsOfFiles(requestId);
    if (urlsOfFits.Data == null)
        Console.WriteLine("Data = null");
    else
        GetFitsFiles(urlsOfFits);
}
catch (UnknowException ex)
{
    Console.WriteLine(ex.Message);
}



void GetFitsFiles(UrlsOfFitsResponse urlsOfFitsResponse)
{
    string baseUrl = "https://jsoc1.stanford.edu/";
    string dir = urlsOfFitsResponse.Dir;
    for (int i = 0; i < urlsOfFitsResponse.Data.Length; i++)
    {
        FitsFileUrl fits = urlsOfFitsResponse.Data[i];
        string filename = fits.Filename;

        string url = baseUrl + Path.Combine(dir, filename);
        Console.WriteLine($"i = {i}. Url = {url}");
    }
}