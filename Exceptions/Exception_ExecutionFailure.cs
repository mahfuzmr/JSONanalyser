namespace JSONanalyser.Exceptions
{
    public class Exception_ExecutionFailure : ApplicationException
    {
        public Exception_ExecutionFailure(string name, object key) : base($"{name} could not retrieve data from url ({key}) or data was not found")
        {

        }
    }
}
