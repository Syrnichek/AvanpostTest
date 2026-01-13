using TR.Connector.Application.DTO;
using TR.Connector.Application.DTO.Responses;
using TR.Connector.Application.Mappers;
using TR.Connector.Domian.DataModels;
using TR.Connector.Domian.Entities;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Application.Services;

public class UserService : IUserService
{
    private readonly IHttpClientService _httpClient;

    public UserService(
        IHttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsUserExistsAsync(string userLogin, CancellationToken cancellationToken)
    {
        var userResponse = await _httpClient.GetAsync<UserResponse>($"api/v1/users/all", cancellationToken);
        var user = userResponse.Data.FirstOrDefault(_ => _.Login == userLogin);
        return user != null;
    }

    public async Task CreateUserAsync(UserCreateRequest user, CancellationToken cancellationToken)
    {
        var createUserDto = UserMapper.MapToCreateUserDto(user);
        await _httpClient.PostAsync<CreateUserDto, object>("api/v1/users/create", createUserDto, cancellationToken);
    }

    public async Task<UserPropertyData> GetUserDataAsync(string userLogin, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync<UserPropertyResponse>($"api/v1/users/{userLogin}",
            cancellationToken);
        return response.Data;
    }
}