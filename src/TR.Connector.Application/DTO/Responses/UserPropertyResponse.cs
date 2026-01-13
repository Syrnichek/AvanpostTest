using TR.Connector.Domian.DataModels;

namespace TR.Connector.Application.DTO.Responses;

internal sealed record UserPropertyResponse(
    UserPropertyData Data,
    bool Success,
    int Count
    );