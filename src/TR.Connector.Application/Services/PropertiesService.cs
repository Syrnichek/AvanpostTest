using TR.Connector.Domian.DataModels;
using TR.Connector.Domian.Entities;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Application.Services;

public class PropertiesService : IPropertiesService
{
    private readonly IUserStatusValidator _userStatusValidator;
    
    private readonly IUserService _userService;
    
    public PropertiesService(
        IUserStatusValidator userStatusValidator,
        IUserService userService)
    {
        _userStatusValidator = userStatusValidator;
        _userService = userService;
    }
    
    public Task<IReadOnlyCollection<Property>> GetAllProperties()
    {
        return Task.FromResult<IReadOnlyCollection<Property>>(UserPropertyData.GetProperties());
    }

    public async Task<IReadOnlyCollection<UserProperty>> GetUserPropertiesAsync(
        string userLogin, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserDataAsync(userLogin, cancellationToken);
        await _userStatusValidator.EnsureUserIsUnlockedAsync(user.Login, cancellationToken);

        return user.ToUserProperties().ToList();
    }

    public async Task UpdateUserPropertiesAsync(IEnumerable<UserProperty> properties, string userLogin,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserDataAsync(userLogin, cancellationToken);
        foreach (var property in properties)
        {
            UpdateProperty(property, user);
        }
    }
    
    private void UpdateProperty(UserProperty property,UserPropertyData user)
    {
        switch (property.Name.ToLowerInvariant())
        {
            case "lastname":
                user.LastName = property.Value?.ToString();
                break;
            case "firstname":
                user.FirstName = property.Value?.ToString();
                break;
            case "middlename":
                user.MiddleName = property.Value?.ToString();
                break;
            case "telephonenumber":
                user.TelephoneNumber = property.Value?.ToString();
                break;
            case "islead":
                if (bool.TryParse(property.Value?.ToString(), out bool isLead))
                    user.IsLead = isLead;
                break;
            case "status":
                user.Status = property.Value?.ToString();
                break;
        }
    }
}