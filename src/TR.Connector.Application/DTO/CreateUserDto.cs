using TR.Connector.Domian.DataModels;

namespace TR.Connector.Application.DTO;

public record CreateUserDto : UserPropertyData
{
    public required string Password { get; set; }
}
