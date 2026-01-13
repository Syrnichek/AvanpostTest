using TR.Connector.Domian.Entities;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector;

/// <summary>
/// Основной класс коннектора для работы с системой управления пользователями,
/// правами доступа и свойствами.
/// </summary>
public class Connector : IConnector, IDisposable
{
    private ServiceContext? _context;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    
    /// <summary>
    /// Создает новый экземпляр коннектора. <see cref="StartUp"/>.
    /// </summary>
    public Connector()
    {
    }

    /// <summary>
    /// Инициализирует коннектор с указанной строкой подключения.
    /// </summary>
    /// <param name="connectionString">Строка подключения к базе данных.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    public async Task StartUp(string connectionString, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            _context?.Dispose();
            _context = await ServiceContextFactory.Create(connectionString, cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Обновляет пользовательские свойства для указанного пользователя.
    /// </summary>
    /// <param name="properties">Коллекция свойств для установки.</param>
    /// <param name="userLogin">Логин пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    public async Task UpdateUserProperties(IReadOnlyCollection<UserProperty> properties, string userLogin,
        CancellationToken cancellationToken)
    {
        var context = GetContext();
        await context.PropertiesService.UpdateUserPropertiesAsync(properties, userLogin, cancellationToken);
    }

    /// <summary>
    /// Возвращает доступные права доступа.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    /// <returns>Коллекция всех прав доступа, определенных в системе.</returns>
    public async Task<IReadOnlyCollection<Permission>> GetAllPermissions(CancellationToken cancellationToken)
    {
        var context = GetContext();
        return await context.PermissionService.GetAllPermissionsAsync(cancellationToken);
    }

    /// <summary>
    /// Возвращает права доступа, назначенные конкретному пользователю.
    /// </summary>
    /// <param name="userLogin">Логин пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    /// <returns>Коллекция идентификаторов прав, назначенных пользователю.</returns>
    public async Task<IReadOnlyCollection<string>> GetUserPermissions(string userLogin,
        CancellationToken cancellationToken)
    {
        var context = GetContext();
        return await context.PermissionService.GetUserPermissionsAsync(userLogin, cancellationToken);
    }

    /// <summary>
    /// Добавляет права доступа пользователю.
    /// </summary>
    /// <param name="userLogin">Логин пользователя.</param>
    /// <param name="rightIds">Коллекция идентификаторов прав для добавления.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    public async Task AddUserPermissions(string userLogin, IReadOnlyCollection<string> rightIds,
        CancellationToken cancellationToken)
    {
        var context = GetContext();
        await context.PermissionService.AddUserPermissionsAsync(userLogin, rightIds, cancellationToken);
    }

    /// <summary>
    /// Удаляет права доступа у пользователя.
    /// </summary>
    /// <param name="userLogin">Логин пользователя.</param>
    /// <param name="rightIds">Коллекция идентификаторов прав для удаления.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    public async Task RemoveUserPermissions(string userLogin, IReadOnlyCollection<string> rightIds,
        CancellationToken cancellationToken)
    {
        var context = GetContext();
        await context.PermissionService.RemoveUserPermissionsAsync(userLogin, rightIds, cancellationToken);
    }

    /// <summary>
    /// Возвращает все свойства, доступные для настройки у пользователей.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    /// <returns>Коллекция всех свойств, определенных в системе.</returns>
    public async Task<IReadOnlyCollection<Property>> GetAllProperties(CancellationToken cancellationToken)
    {
        var context = GetContext();
        return await context.PropertiesService.GetAllProperties();
    }

    /// <summary>
    /// Возвращает значения свойств для конкретного пользователя.
    /// </summary>
    /// <param name="userLogin">Логин пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    /// <returns>Коллекция пользовательских свойств с их значениями.</returns>
    public async Task<IReadOnlyCollection<UserProperty>> GetUserProperties(string userLogin,
        CancellationToken cancellationToken)
    {
        var context = GetContext();
        return await context.PropertiesService.GetUserPropertiesAsync(userLogin, cancellationToken);
    }

    /// <summary>
    /// Проверяет существование пользователя в системе по логину.
    /// </summary>
    /// <param name="userLogin">Логин пользователя для проверки.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    /// <returns>true, если пользователь существует; иначе false.</returns>
    public async Task<bool> IsUserExists(string userLogin, CancellationToken cancellationToken)
    {
        var context = GetContext();
        return await context.UserService.IsUserExistsAsync(userLogin, cancellationToken);
    }

    /// <summary>
    /// Создает нового пользователя в системе.
    /// </summary>
    /// <param name="user">Данные для создания пользователя.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
    public async Task CreateUser(UserCreateRequest user, CancellationToken cancellationToken)
    {
        var context = GetContext();
        await context.UserService.CreateUserAsync(user, cancellationToken);
    }

    /// <summary>
    /// Получает текущий экземпляр ServiceContext.
    /// </summary>
    /// <returns>Экземпляр ServiceContext.</returns>
    private ServiceContext GetContext() => _context ?? throw new InvalidOperationException(
        "Connector is not initialized. Call StartUp() first.");

    public void Dispose()
    {
        _context?.Dispose();
    }
}