using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TR.Connector.Application.Helpers;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Application.Implementations;

public class HttpClientService : IHttpClientService, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IApiConfiguration _config;

    public HttpClientService(IApiConfiguration config)
    {
        _config = config;
        
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(config.BaseUrl)
        };
        
        UpdateAuthorizationHeader();
    }

    private void UpdateAuthorizationHeader()
    {
        if (!string.IsNullOrEmpty(_config.AccessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", _config.AccessToken);
        }
    }

    public async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken) where T : class
    {
        var response = await _httpClient.GetAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return CustomJson.DeserializeRequired<T>(content, typeof(T));
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken) 
        where TResponse : class
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        return CustomJson.DeserializeRequired<TResponse>(responseContent, typeof(TResponse));
    }

    public async Task PutAsync(string endpoint, CancellationToken cancellationToken, object? content)
    {
        StringContent? stringContent = null;
        if (content != null)
        {
            var json = CustomJson.Serialize(content);
            stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        var response = await _httpClient.PutAsync(endpoint, stringContent, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string endpoint, CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose() => _httpClient.Dispose();
}