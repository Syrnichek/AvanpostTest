namespace TR.Connector.Domian.Interfaces;

public interface IUserStatusValidator
{
    Task EnsureUserIsUnlockedAsync(string userLogin, CancellationToken cancellationToken);
}
