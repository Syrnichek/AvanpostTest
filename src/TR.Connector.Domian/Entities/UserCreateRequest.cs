using TR.Connector.Domian.Exceptions.Domian;

namespace TR.Connector.Domian.Entities;

public sealed class UserCreateRequest
{
    public UserCreateRequest(string login, string hashPassword)
    {
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(hashPassword))
            throw new RequiredFieldException(string.IsNullOrWhiteSpace(login) ? "login" : "password");
        HashPassword = hashPassword;
        Login = login;
        Properties = Array.Empty<UserProperty>();
    }
    
    public string Login { get; set; }
    
    public string HashPassword { get; set; }
    
    public IEnumerable<UserProperty> Properties { get; set; }
}