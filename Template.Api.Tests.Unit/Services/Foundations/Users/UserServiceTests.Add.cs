using FluentAssertions;
using Moq;
using Template.Api.Models.Foundation.User;
using Template.Api.Models.Foundation.User.Exceptions;

namespace Template.Api.Tests.Unit.Services.Foundations.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldAddUserAsync()
    {
        // Given — a valid user that doesn't already exist
        User randomUser = CreateRandomUser();
        User inputUser = randomUser;
        User expectedUser = inputUser;

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(inputUser.Id))
                .ReturnsAsync((User?)null);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(inputUser))
                .ReturnsAsync(expectedUser);

        // When — we add the user
        User actualUser =
            await this.userService.AddUserAsync(inputUser);

        // Then — the returned user should match what was inserted
        actualUser.Should().BeEquivalentTo(expectedUser);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(inputUser.Id),
                Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(inputUser),
                Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenUserIsNullAsync()
    {
        // Given — a null user
        User? nullUser = null;

        // When — we try to add it
        Func<Task> addUserTask = async () =>
            await this.userService.AddUserAsync(nullUser!);

        // Then — it should throw a UserValidationException wrapping NullUserException
        await addUserTask.Should().ThrowAsync<UserValidationException>()
            .Where(ex => ex.InnerException is NullUserException);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(It.IsAny<User>()),
                Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenIdIsEmptyAsync()
    {
        // Given — a user with an empty Id
        User invalidUser = CreateRandomUser();
        invalidUser.Id = Guid.Empty;

        // When — we try to add it
        Func<Task> addUserTask = async () =>
            await this.userService.AddUserAsync(invalidUser);

        // Then — it should throw a UserValidationException wrapping InvalidUserException
        await addUserTask.Should().ThrowAsync<UserValidationException>()
            .Where(ex => ex.InnerException is InvalidUserException);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(It.IsAny<User>()),
                Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddWhenFirstNameIsInvalidAsync(string? invalidFirstName)
    {
        // Given — a user with an invalid first name
        User invalidUser = CreateRandomUser();
        invalidUser.FirstName = invalidFirstName!;

        // When
        Func<Task> addUserTask = async () =>
            await this.userService.AddUserAsync(invalidUser);

        // Then
        await addUserTask.Should().ThrowAsync<UserValidationException>()
            .Where(ex => ex.InnerException is InvalidUserException);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(It.IsAny<User>()),
                Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddWhenEmailIsInvalidAsync(string? invalidEmail)
    {
        // Given — a user with an invalid email
        User invalidUser = CreateRandomUser();
        invalidUser.Email = invalidEmail!;

        // When
        Func<Task> addUserTask = async () =>
            await this.userService.AddUserAsync(invalidUser);

        // Then
        await addUserTask.Should().ThrowAsync<UserValidationException>()
            .Where(ex => ex.InnerException is InvalidUserException);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(It.IsAny<User>()),
                Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddWhenUserAlreadyExistsAsync()
    {
        // Given — a user that already exists in the database
        User randomUser = CreateRandomUser();
        User alreadyExistingUser = randomUser;

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(alreadyExistingUser.Id))
                .ReturnsAsync(alreadyExistingUser);

        // When — we try to add the same user
        Func<Task> addUserTask = async () =>
            await this.userService.AddUserAsync(alreadyExistingUser);

        // Then — it should throw a UserValidationException wrapping AlreadyExistsUserException
        await addUserTask.Should().ThrowAsync<UserValidationException>()
            .Where(ex => ex.InnerException is AlreadyExistsUserException);

        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(alreadyExistingUser.Id),
                Times.Once);

        this.storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(It.IsAny<User>()),
                Times.Never);

        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}
