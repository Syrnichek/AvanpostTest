using TR.Connector.Domian.Entities;

namespace TR.Connector.Domian.Interfaces;

public interface IConnector
{
    Task StartUp(string connectionString, CancellationToken cancellationToken);

    Task CreateUser(UserCreateRequest user, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Property>> GetAllProperties(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<UserProperty>> GetUserProperties(string userLogin, CancellationToken cancellationToken);

    Task<bool> IsUserExists(string userLogin, CancellationToken cancellationToken);

    Task UpdateUserProperties(IReadOnlyCollection<UserProperty> properties, string userLogin,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Permission>> GetAllPermissions(CancellationToken cancellationToken);

    Task AddUserPermissions(string userLogin, IReadOnlyCollection<string> rightIds,
        CancellationToken cancellationToken);

    Task RemoveUserPermissions(string userLogin, IReadOnlyCollection<string> rightIds,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<string>> GetUserPermissions(string userLogin, CancellationToken cancellationToken);
}
