using TR.Connector.Domian.Entities;

namespace TR.Connector.Domian.Interfaces;

public interface IPermissionService
{
    Task<IReadOnlyCollection<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<string>> GetUserPermissionsAsync(string userLogin, CancellationToken cancellationToken);

    Task AddUserPermissionsAsync(string userLogin, IReadOnlyCollection<string> rightIds,
        CancellationToken cancellationToken);

    Task RemoveUserPermissionsAsync(string userLogin, IReadOnlyCollection<string> rightIds,
        CancellationToken cancellationToken);
}
