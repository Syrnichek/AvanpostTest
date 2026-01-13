using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Infrastructure.Implementations;

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

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(responseContent);
    }

    public async Task PutAsync(string endpoint, object? content = null)
    {
        StringContent? stringContent = null;
        if (content != null)
        {
            var json = JsonSerializer.Serialize(content);
            stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        var response = await _httpClient.PutAsync(endpoint, stringContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }

    public void Dispose() => _httpClient.Dispose();
}