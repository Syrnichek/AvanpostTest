using TR.Connector.Domian.Exceptions.Domian;

namespace TR.Connector.Domian.Entities;

public sealed class Property
{
    public Property(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidNameException("name");
        }
        Name = name;
        Description = description;
    }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}