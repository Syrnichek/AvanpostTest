using TR.Connector.Domian.DataModels;

namespace TR.Connector.Application.DTO.Responses;

internal sealed record UserResponse(
    IReadOnlyCollection<UserResponseData> Data,
    bool Success,
    int Count
    );