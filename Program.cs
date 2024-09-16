Console.WriteLine("Welcome to JSOC Parser!");

var jsocApi = new JsocApi();

// todo: Возможно нужно еще вызывать операцию check?
var fromTime = new DateTime(2015, 5, 27);
try
{
    string requestId = await jsocApi.Submit(fromTime, 85, 94);
    Console.WriteLine($"RequestId = {requestId}. Waiting...");
    await jsocApi.Check();
    Console.WriteLine("Checking parametrs...");
    UrlsOfFitsResponse urlsOfFits = await jsocApi.GetUrlsOfFiles(requestId);
    Console.WriteLine($"Fetch. RequestId = {requestId}");
    GetFitsFiles(urlsOfFits);
}
catch (ProcessingExportRequestException ex)
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