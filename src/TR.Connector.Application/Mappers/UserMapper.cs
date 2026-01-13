using TR.Connector.Application.DTO;
using TR.Connector.Domian.Entities;

namespace TR.Connector.Application.Mappers;

public static class UserMapper
{
    public static CreateUserDto MapToCreateUserDto(UserCreateRequest user)
    {
        var properties = user.Properties.ToDictionary(
            p => p.Name.ToLower(),
            p => p.Value);

        return new CreateUserDto
        {
            Login = user.Login,
            Password = user.HashPassword,
            LastName = properties["lastname"],
            FirstName = properties["firstname"],
            MiddleName = properties["middleName"],
        };
    }
}
