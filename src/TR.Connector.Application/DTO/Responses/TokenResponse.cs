using TR.Connector.Domian.DataModels;

namespace TR.Connector.Application.DTO.Responses;

internal sealed record TokenResponse(
    TokenResponseData Data,
    bool Success
    );