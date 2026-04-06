using Moq;
using Template.Api.Brokers.Logging;
using Template.Api.Models.Foundation.User;
using Template.Api.Services.Foundations.Users;

namespace Template.Api.Tests.Unit.Services.Foundations.Users;

/// <summary>
/// Setup class for UserService tests.
/// Creates mocked dependencies and the service under test.
/// Each partial class file contains tests for a specific method.
/// </summary>
public partial class UserServiceTests
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IUserService userService;

    public UserServiceTests()
    {
        this.storageBrokerMock = new Mock<IStorageBroker>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();

        this.userService = new UserService(
            storageBroker: this.storageBrokerMock.Object,
            loggingBroker: this.loggingBrokerMock.Object);
    }

    // Helper to create a valid user with all required fields populated.
    private static User CreateRandomUser() => new()
    {
        Id = Guid.NewGuid(),
        OrganizationId = Guid.NewGuid(),
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        Phone = "555-1234",
        Role = "User",
        RegistrationDate = DateTime.UtcNow,
        IsActive = true,
        PasswordHash = "hashedpassword123"
    };
}
