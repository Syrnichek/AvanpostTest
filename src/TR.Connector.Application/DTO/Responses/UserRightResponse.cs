using TR.Connector.Domian.DataModels;

namespace TR.Connector.Application.DTO.Responses;

internal sealed record UserRightResponse(
    IReadOnlyCollection<RightResponseData> Data,
    bool Success,
    int Count
    );