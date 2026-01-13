using TR.Connector.Domian.DataModels;
using TR.Connector.Domian.Entities;

namespace TR.Connector.Domian.Interfaces;

public interface IUserService
{
    Task<bool> IsUserExistsAsync(string userLogin, CancellationToken cancellationToken);

    Task CreateUserAsync(UserCreateRequest user, CancellationToken cancellationToken);

    Task<UserPropertyData> GetUserDataAsync(string userLogin, CancellationToken cancellationToken);
}