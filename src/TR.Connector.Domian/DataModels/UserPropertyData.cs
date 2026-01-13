using TR.Connector.Domian.Entities;

namespace TR.Connector.Domian.DataModels;

public record UserPropertyData
{
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }
    public string? TelephoneNumber { get; set; }
    public bool IsLead { get; set; }
    public required string Login { get; set; }
    public string? Status { get; set; }
    
    private static readonly Lazy<IReadOnlyList<Property>> Properties = new(() =>
    {
        return new[]
        {
            new Property(nameof(LastName), "Фамилия"),
            new Property(nameof(FirstName), "Имя"),
            new Property(nameof(MiddleName), "Отчество"),
            new Property(nameof(TelephoneNumber), "Телефон"),
            new Property(nameof(IsLead), "Является руководителем"),
            new Property(nameof(Status), "Статус")
        }.AsReadOnly();
    });
    
    public static IReadOnlyList<Property> GetProperties() => Properties.Value;
    
    public IEnumerable<UserProperty> ToUserProperties()
    {
        yield return new UserProperty(nameof(LastName), LastName);
        yield return new UserProperty(nameof(FirstName), FirstName);
        yield return new UserProperty(nameof(MiddleName), MiddleName);
        yield return new UserProperty(nameof(TelephoneNumber), TelephoneNumber);
        yield return new UserProperty(nameof(IsLead), IsLead.ToString());
        yield return new UserProperty(nameof(Status), Status);
    }
}