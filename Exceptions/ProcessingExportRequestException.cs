public class ProcessingExportRequestException : Exception
{
    public ProcessingExportRequestException(int code) : base($"Export error with code = {code}")
    {

    }
}