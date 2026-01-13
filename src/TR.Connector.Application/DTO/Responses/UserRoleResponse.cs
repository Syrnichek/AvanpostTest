using TR.Connector.Domian.DataModels;

namespace TR.Connector.Application.DTO.Responses;

internal sealed record UserRoleResponse(
    IReadOnlyCollection<RoleResponseData> Data,
    bool Success,
    int Count
    );