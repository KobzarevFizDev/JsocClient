public class NotCorrectFetchRequest : Exception
{
    public NotCorrectFetchRequest(int statusCode)
    : base($"Not correct fetch request. Status code = {statusCode}")
    {

    }
}