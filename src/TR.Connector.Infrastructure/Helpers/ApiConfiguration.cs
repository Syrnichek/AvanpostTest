using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Infrastructure.Helpers;

public class ApiConfiguration : IApiConfiguration
{
    public string BaseUrl { get; private set; }
    public string Login { get; private set; }
    public string Password { get; private set; }
    public string AccessToken { get; set; }

    public void ParseConnectionString(string connectionString)
    {
        foreach (var item in connectionString.Split(';'))
        {
            var parts = item.Split('=', 2);
            if (parts.Length != 2) continue;

            switch (parts[0].ToLower())
            {
                case "url":
                    BaseUrl = parts[1];
                    break;
                case "login":
                    Login = parts[1];
                    break;
                case "password":
                    Password = parts[1];
                    break;
            }
        }
    }
}