namespace TR.Connector.Domian.Interfaces;

public interface IApiConfiguration
{
    void ParseConnectionString(string connectionString);

    string BaseUrl { get; }

    string Login { get; }

    string Password { get; }

    string AccessToken { get; set; }
}
