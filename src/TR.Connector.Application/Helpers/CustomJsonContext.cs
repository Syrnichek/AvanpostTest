using System.Text.Json.Serialization;
using TR.Connector.Application.DTO;
using TR.Connector.Application.DTO.Responses;

namespace TR.Connector.Application.Helpers;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
    WriteIndented = false,
    GenerationMode = JsonSourceGenerationMode.Serialization | JsonSourceGenerationMode.Metadata,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    IncludeFields = false
)]
[JsonSerializable(typeof(RoleResponse))]
[JsonSerializable(typeof(UserRoleResponse))]
[JsonSerializable(typeof(UserResponse))]
[JsonSerializable(typeof(UserPropertyResponse))]
[JsonSerializable(typeof(CreateUserDto))]
internal partial class CustomJsonContext : JsonSerializerContext;