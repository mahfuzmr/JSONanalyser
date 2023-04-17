namespace JSONanalyser.Exceptions
{
    public class Exception_NotFound : ApplicationException
    {
        public Exception_NotFound(string name, object key) : base($"{name} could not retrieve data from url ({key}) or data was not found")
        {

        }
    }
}
