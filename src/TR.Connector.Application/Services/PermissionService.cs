using TR.Connector.Application.DTO.Responses;
using TR.Connector.Domian.Entities;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IHttpClientService _httpClient;
    private readonly IUserStatusValidator _userStatusValidator;

    public PermissionService(
        IHttpClientService httpClient,
        IUserStatusValidator userStatusValidator)
    {
        _httpClient = httpClient;
        _userStatusValidator = userStatusValidator;
    }

    public async Task<IReadOnlyCollection<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken)
    {
        // Получаем IT-роли
        var rolesResponse = await _httpClient.GetAsync<RoleResponse>("api/v1/roles/all", cancellationToken);
        var itRolePermissions = rolesResponse.Data
            .Select(responseData => new Permission($"ItRole,{responseData.Id}", responseData.Name, responseData.CorporatePhoneNumber));

        // Получаем права
        var rightsResponse = await _httpClient.GetAsync<UserRightResponse>("api/v1/rights/all", cancellationToken);
        var rightPermissions = rightsResponse.Data
            .Select(responseData => new Permission($"RequestRight,{responseData.Id}", responseData.Name, responseData.Users.ToString()));

        return itRolePermissions
            .Concat(rightPermissions)
            .ToList()
            .AsReadOnly();
    }

    public async Task<IReadOnlyCollection<string>> GetUserPermissionsAsync(string userLogin,
        CancellationToken cancellationToken)
    {
        // Получаем IT-роли пользователя
        var rolesResponse = await _httpClient.GetAsync<UserRoleResponse>(
            $"api/v1/users/{userLogin}/roles", cancellationToken);
        
        // Получаем права пользователя
        var rightsResponse = await _httpClient.GetAsync<UserRightResponse>(
            $"api/v1/users/{userLogin}/rights", cancellationToken);

        var roleIds = rolesResponse.Data.Select(responseData => $"ItRole,{responseData.Id}");
        var rightIds = rightsResponse.Data.Select(responseData => $"RequestRight,{responseData.Id}");

        return roleIds.Concat(rightIds).ToList().AsReadOnly();
    }

    public async Task AddUserPermissionsAsync(string userLogin,
        IReadOnlyCollection<string> rightIds, CancellationToken cancellationToken)
    {
        await _userStatusValidator.EnsureUserIsUnlockedAsync(userLogin, cancellationToken);
        
        foreach (var rightId in rightIds)
        {
            await AddSinglePermissionAsync(userLogin, rightId, cancellationToken);
        }
    }

    public async Task RemoveUserPermissionsAsync(string userLogin,
        IReadOnlyCollection<string> rightIds, CancellationToken cancellationToken)
    {
        await _userStatusValidator.EnsureUserIsUnlockedAsync(userLogin, cancellationToken);
        
        foreach (var rightId in rightIds)
        {
            await RemoveSinglePermissionAsync(userLogin, rightId, cancellationToken);
        }
    }

    private async Task AddSinglePermissionAsync(string userLogin, string rightId, CancellationToken cancellationToken)
    {
        var (type, id) = ParsePermissionId(rightId);
        
        var endpoint = type switch
        {
            "ItRole" => $"api/v1/users/{userLogin}/add/role/{id}",
            "RequestRight" => $"api/v1/users/{userLogin}/add/right/{id}",
            _ => throw new ArgumentException($"Тип доступа {type} не определен")
        };
        
        await _httpClient.PutAsync(endpoint, cancellationToken);
    }

    private async Task RemoveSinglePermissionAsync(string userLogin, string rightId, CancellationToken cancellationToken)
    {
        var (type, id) = ParsePermissionId(rightId);
        
        var endpoint = type switch
        {
            "ItRole" => $"api/v1/users/{userLogin}/drop/role/{id}",
            "RequestRight" => $"api/v1/users/{userLogin}/drop/right/{id}",
            _ => throw new ArgumentException($"Тип доступа {type} не определен")
        };
        
        await _httpClient.DeleteAsync(endpoint, cancellationToken);
    }

    private (string Type, string Id) ParsePermissionId(string permissionId)
    {
        var parts = permissionId.Split(',');
        if (parts.Length != 2)
        {
            throw new FormatException($"Некорректный формат permissionId: {permissionId}");
        }

        return (parts[0], parts[1]);
    }
}
