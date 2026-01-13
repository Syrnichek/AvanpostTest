using TR.Connector.Domian.Entities;

namespace TR.Connector.Infrastructure.Helpers;

public class PropertyReflector
{
    public IEnumerable<Property> GetPropertiesFromType<T>()
    {
        return typeof(T)
            .GetProperties()
            .Where(p => p.Name != "login")
            .Select(p => new Property(p.Name, p.Name));
    }

    public static IEnumerable<UserProperty> ExtractProperties(object obj)
    {
        return obj.GetType()
            .GetProperties()
            .Select(p => new UserProperty(p.Name, p.GetValue(obj)?.ToString()));
    }
}