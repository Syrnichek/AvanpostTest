namespace TR.Connector.Domian.Exceptions.Domian;

public class RequiredFieldException : TaskRecruitingException
{
    public RequiredFieldException(string message) : base(message)
    {
    }

    public RequiredFieldException(string message, Exception innerException) : base(message, innerException)
    {
    }
}