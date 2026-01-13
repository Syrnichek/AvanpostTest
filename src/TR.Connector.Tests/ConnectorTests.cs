using TR.Connector.Domian.Entities;
using TR.Connector.Domian.Interfaces;

namespace TR.Connector.Tests
{
    public class ConnectorTests
    {
        private readonly IConnector _connector;
        private readonly string _connectorString = "url=http://localhost:5000;login=login;password=password";
        private static readonly CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _token = CancelTokenSource.Token;
        
        public ConnectorTests()
        {
            _connector = new Connector();
            _connector.StartUp(_connectorString, _token);
        }

        [Fact]
        public async Task GetAllPermissions_Ok()
        {
            var permissions = await _connector.GetAllPermissions(_token);
            Assert.NotNull(permissions);

            var ItRole9 = permissions.FirstOrDefault(_ => _.Name == "ITRole9");
            Assert.Equal("ItRole,9", ItRole9.Id);

            var RequestRight5 = permissions.FirstOrDefault(_ => _.Name == "RequestRight5");
            Assert.Equal("RequestRight,5", RequestRight5.Id);
        }

        [Fact]
        public async Task GetUserPermissions_Ok()
        {
            var login = "Login3";
            var permissions = await _connector.GetUserPermissions(login, _token);

            Assert.NotNull(permissions);
            Assert.NotNull(permissions.FirstOrDefault(_ => _.Contains("ItRole")));
            Assert.NotNull(permissions.FirstOrDefault(_ => _.Contains("RequestRight")));
        }

        /*[Fact]
        public void GetUserPermissions1_Ok()
        {
            var login = "Login4"; //lock

            //������ �� �������. ������ ������ ������ "������������ Login4 ������������".
            var permissions = _connector.GetUserPermissions(login);

            Assert.NotNull(permissions);
            Assert.Empty(permissions);
        }*/

        [Fact]
        public async Task Add_Drop_Permissions_Ok()
        {
            var login = "Login7";
            var userRole = "ItRole,5";
            var userRight = "RequestRight,5";
            await _connector.AddUserPermissions(login, new List<string>(){userRole, userRight}, _token);

            var userPermissions = await _connector.GetUserPermissions(login, _token);
            Assert.NotNull(userPermissions.FirstOrDefault(_ => _.Contains(userRole)));
            Assert.NotNull(userPermissions.FirstOrDefault(_ => _.Contains(userRight)));

            await _connector.RemoveUserPermissions(login, new List<string>(){userRole, userRight}, _token);

            userPermissions = await _connector.GetUserPermissions(login, _token);
            Assert.Null(userPermissions.FirstOrDefault(_ => _.Contains(userRole)));
            Assert.Null(userPermissions.FirstOrDefault(_ => _.Contains(userRight)));
        }

        [Fact]
        public async Task GetAllProperties_Ok()
        {
            var allProperties = await _connector.GetAllProperties(_token);

            Assert.NotNull(allProperties);
            Assert.NotNull(allProperties.FirstOrDefault(_ => _.Name.Contains("isLead")));
        }

        [Fact]
        public async Task Get_UpdateUserProperties_Ok()
        {
            var login = "Login3";
            var userProperties = await _connector.GetUserProperties(login, _token);
            Assert.NotNull(userProperties);

            Assert.Equal("FirstName3", userProperties.FirstOrDefault(_ => _.Name == "firstName").Value);
            Assert.Equal("TelephoneNumber3", userProperties.FirstOrDefault(_ => _.Name == "telephoneNumber").Value);

            var userProps = new List<UserProperty>()
            {
                new UserProperty("firstName", "FirstName13"),
                new UserProperty("telephoneNumber", "TelephoneNumber13"),
            };
            await _connector.UpdateUserProperties(userProps, login, _token);


            userProperties = await _connector.GetUserProperties(login, _token);
            Assert.NotNull(userProperties);

            Assert.Equal("FirstName13", userProperties.FirstOrDefault(_ => _.Name == "firstName").Value);
            Assert.Equal("TelephoneNumber13", userProperties.FirstOrDefault(_ => _.Name == "telephoneNumber").Value);
        }

        [Fact]
        public async Task Get_CreateUser_Ok()
        {
            var login = "Login100";

            var isUser = await _connector.IsUserExists(login, _token);
            Assert.False(isUser);

            var user = new UserCreateRequest(login, "Password100")
            {
                Properties = new List<UserProperty>()
                {
                    new UserProperty("firstName", "FirstName100"),
                    new UserProperty("lastName", ""),
                    new UserProperty("middleName", ""),
                    new UserProperty("telephoneNumber", ""),
                    new UserProperty("isLead", ""),
                }
            };

            await _connector.CreateUser(user, _token);

            isUser = await _connector.IsUserExists(login, _token);
            Assert.True(isUser);
        }
    }
}