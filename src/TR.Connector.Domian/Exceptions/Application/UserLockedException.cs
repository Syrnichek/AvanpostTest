namespace TR.Connector.Domian.Exceptions.Application;

public class UserLockedException : TaskRecruitingException
{
    public UserLockedException(string message) : base(message)
    {
    }

    public UserLockedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}