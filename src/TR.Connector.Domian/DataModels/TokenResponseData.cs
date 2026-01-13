namespace TR.Connector.Domian.DataModels;

public record TokenResponseData(
    string AccessToken,
    int ExpiresIn);