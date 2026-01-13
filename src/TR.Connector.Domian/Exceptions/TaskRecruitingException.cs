namespace TR.Connector.Domian.Exceptions;

public class TaskRecruitingException : Exception
{
    protected TaskRecruitingException(string message) : base(message) { }

    protected TaskRecruitingException(string message, Exception innerException) : base(message, innerException) { }
}
