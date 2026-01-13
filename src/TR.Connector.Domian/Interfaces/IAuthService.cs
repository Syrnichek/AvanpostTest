namespace TR.Connector.Domian.Interfaces;

public interface IAuthService
{
    Task AuthenticateAsync(string connectionString, CancellationToken cancellationToken);
}
