using TR.Connector.Domian.Interfaces;

namespace TR.Connector;

public class ServiceContext(
    IPermissionService permissionService,
    IUserService userService,
    IHttpClientService httpClient,
    IPropertiesService propertiesService)
    : IDisposable
{
    public void Dispose()
    {
        HttpClient.Dispose();
    }

    public IPermissionService PermissionService => permissionService;

    public IUserService UserService => userService;

    private IHttpClientService HttpClient => httpClient;

    public IPropertiesService PropertiesService => propertiesService;

}