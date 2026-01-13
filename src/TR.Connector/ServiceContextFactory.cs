using Microsoft.Extensions.Logging;
using TR.Connector.Application.Helpers;
using TR.Connector.Application.Identity;
using TR.Connector.Application.Implementations;
using TR.Connector.Application.Services;

namespace TR.Connector;

public class ServiceContextFactory
{
    public static async Task<ServiceContext> Create(string connectionString, CancellationToken cancellationToken)
    {
        var config = new ApiConfiguration();
        config.ParseConnectionString(connectionString);

        var httpClient = new HttpClientService(config);

        var authService = new AuthService(config, httpClient);
        await authService.AuthenticateAsync(connectionString, cancellationToken);

        var userStatusValidator = new UserStatusValidator(httpClient,
            new Logger<UserStatusValidator>(new LoggerFactory()));
        var permissionService = new PermissionService(httpClient, userStatusValidator);
        var userService = new UserService(httpClient);
        var propertiesService = new PropertiesService(userStatusValidator, userService);

        return new ServiceContext(
            permissionService,
            userService,
            httpClient,
            propertiesService
        );
    }
}