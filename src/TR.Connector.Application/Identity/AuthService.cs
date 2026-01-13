using TR.Connector.Application.DTO.Requests;
using TR.Connector.Application.DTO.Responses;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Application.Identity;

public sealed class AuthService : IAuthService
{
    private readonly IApiConfiguration _config;
    private readonly IHttpClientService _httpClient;

    public AuthService(
        IApiConfiguration config,
        IHttpClientService httpClient)
    {
        _config = config;
        _httpClient = httpClient;
    }

    public async Task AuthenticateAsync(string connectionString, CancellationToken cancellationToken)
    {
        var authRequest = new AuthRequest(_config.Login, _config.Password);
        var tokenResponse = await _httpClient.PostAsync<AuthRequest, TokenResponse>(
            "api/v1/login", 
            authRequest, cancellationToken);

        _config.AccessToken = tokenResponse.Data.AccessToken;
    }
}