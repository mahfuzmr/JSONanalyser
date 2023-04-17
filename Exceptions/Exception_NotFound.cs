namespace JSONanalyser.Exceptions
{
    public class Exception_NotFound : ApplicationException
    {
        public Exception_NotFound(string name, object key) : base($"{name} with amount ({key}) was not found")
        {

        }
    }
}
