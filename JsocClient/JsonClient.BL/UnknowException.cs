namespace JsonClient.BL
{
    public class UnknowException : Exception
    {
        public UnknowException(int code) : base($"Something wrong. Error code = {code}")
        {

        }
    }
}
