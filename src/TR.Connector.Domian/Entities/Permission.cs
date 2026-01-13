using TR.Connector.Domian.Exceptions.Domian;

namespace TR.Connector.Domian.Entities;

public sealed class Permission
{
    public Permission(
        string id,
        string name,
        string description)
    {
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(id))
        {
            throw new RequiredFieldException(string.IsNullOrWhiteSpace(name) ? "name" : "id");
        }

        Id = id;
        Name = name;
        Description = description;
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}