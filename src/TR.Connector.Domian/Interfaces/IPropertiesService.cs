using TR.Connector.Domian.Entities;

namespace TR.Connector.Domian.Interfaces;

public interface IPropertiesService
{
    Task<IReadOnlyCollection<Property>> GetAllProperties();

    Task<IReadOnlyCollection<UserProperty>> GetUserPropertiesAsync(string userLogin,
        CancellationToken cancellationToken);

    Task UpdateUserPropertiesAsync(IEnumerable<UserProperty> properties, string userLogin, 
        CancellationToken cancellationToken);
}