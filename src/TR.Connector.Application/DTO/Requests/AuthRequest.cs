namespace TR.Connector.Application.DTO.Requests;

public record AuthRequest(
    string Login,
    string Password
    );