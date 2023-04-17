namespace JSONanalyser.Exceptions
{
    public class UrlDecodeException : Exception
    {
        public UrlDecodeException(string url, Exception innerException) : base($"Failed to decode URL: {url}", innerException ?? innerException)
        {
        }
    }
}
