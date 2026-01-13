namespace TR.Connector.Domian.Exceptions.Domian;

public class InvalidNameException : TaskRecruitingException
{
    public InvalidNameException(string message) : base(message)
    {
    }

    public InvalidNameException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
