namespace TR.Connector.Domian.Exceptions.Application;

public class UserNotFoundException: TaskRecruitingException
{
    public UserNotFoundException(string message) : base(message)
    {
    }

    public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}