Console.WriteLine("Welcome to JSOC Parser!");

var jsocApi = new JsocApi();

var fromTime = new DateTime(2014, 2, 27);
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
    await jsocApi.WaitForCurrentRequestToComplete();
    var linksToFitsFiles = await jsocApi.GetLinksToFitsFiles(requestId);
    await DownloadFitsFiles(linksToFitsFiles);
    Console.WriteLine("End");
}
catch (UnknowException ex)
{
    Console.WriteLine(ex.Message);
}



async Task DownloadFitsFiles(LinksToFitsFiles urlsOfFitsResponse)
{
    string baseUrl = "https://jsoc1.stanford.edu/";
    string dir = urlsOfFitsResponse.Dir;

    string filename = urlsOfFitsResponse.Data[0].Filename;
    string url = baseUrl + Path.Combine(dir, filename);
    Console.WriteLine($"GET({url})");
    using (HttpClient client = new HttpClient())
    {
        using (HttpResponseMessage response = await client.GetAsync(url))
        {
            using (Stream streamForRead = await response.Content.ReadAsStreamAsync())
            {
                string pathToSave = Path.Combine(Environment.CurrentDirectory, filename);
                Console.WriteLine("Save to: " + pathToSave);
                using (FileStream streamForSave = File.Create(pathToSave))
                {
                    await streamForRead.CopyToAsync(streamForSave);
                    streamForSave.Flush();
                }
            }
        }
    }
}


/*for (int i = 0; i < urlsOfFitsResponse.Data.Length; i++)
{
    FitsFileUrl fits = urlsOfFitsResponse.Data[i];
    string filename = fits.Filename;

    string url = baseUrl + Path.Combine(dir, filename);
    Console.WriteLine($"i = {i}. Url = {url}");
}*/