using TR.Connector.Domian.Exceptions.Domian;

namespace TR.Connector.Domian.Entities;

public sealed class UserProperty
{
    public UserProperty(string name, string value)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new InvalidNameException("name");
        }

        Name = name;
        Value = value;
    }

    public string Name { get; set; }

    public string Value { get; set; }
}