namespace TR.Connector.Domian.Interfaces;

public interface IHttpClientService
{
    Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken) where T : class; 

    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint,
        TRequest request, 
        CancellationToken cancellationToken) 
        where TResponse : class;

    Task PutAsync(string endpoint, CancellationToken cancellationToken, object? content = null);

    Task DeleteAsync(string endpoint, CancellationToken cancellationToken);

    void Dispose();
}
