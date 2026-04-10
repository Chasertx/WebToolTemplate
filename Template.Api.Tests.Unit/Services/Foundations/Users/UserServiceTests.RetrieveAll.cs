using FluentAssertions;
using Moq;
using Template.Api.Models.Foundation.User;

namespace Template.Api.Tests.Unit.Services.Foundations.Users;

public partial class UserServiceTests
{
    [Fact]
    public void ShouldRetrieveAllUsers()
    {
        // Given — a list of users in the database
        IQueryable<User> randomUsers = new List<User>
        {
            CreateRandomUser(),
            CreateRandomUser(),
            CreateRandomUser()
        }.AsQueryable();

        IQueryable<User> expectedUsers = randomUsers;

        this.storageBrokerMock.Setup(broker =>
            broker.SelectAllUsers())
                .Returns(expectedUsers);

        // When — we retrieve all users
        IQueryable<User> actualUsers =
            this.userService.RetrieveAllUsers();

        // Then — the returned collection should match
        actualUsers.Should().BeEquivalentTo(expectedUsers);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectAllUsers(),
                Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}
