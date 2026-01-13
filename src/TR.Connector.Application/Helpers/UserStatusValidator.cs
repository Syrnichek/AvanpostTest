using System.Net;
using Microsoft.Extensions.Logging;
using TR.Connector.Application.DTO.Responses;
using TR.Connector.Domian.Exceptions.Application;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Application.Helpers;

public sealed partial class UserStatusValidator(IHttpClientService httpClient, ILogger<UserStatusValidator> logger)
    : IUserStatusValidator
{
    private const string UserStatusKey = "Lock";
    
    public async Task EnsureUserIsUnlockedAsync(string userLogin, CancellationToken cancellationToken)
    {
        try
        {
            var response = await httpClient.GetAsync<UserPropertyResponse>(
                $"api/v1/users/{userLogin}", cancellationToken);

            if (response.Data is null)
            {
                throw new UserNotFoundException(userLogin);
            }

            if (response.Data.Status.Equals(UserStatusKey))
            {
                LogUserLocked(userLogin);
                throw new UserLockedException(response.Data.Login);
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new UserNotFoundException(userLogin);
        }
    }
    
    [LoggerMessage(LogLevel.Debug, "Пользователь {userLogin} залочен")]
    public partial void LogUserLocked(string userLogin);
}