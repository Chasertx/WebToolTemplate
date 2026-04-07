using Template.Api.Models.Foundation.User;
using Template.Api.Models.Foundation.User.Exceptions;

namespace Template.Api.Services.Foundations.Users;

/// <summary>
/// This is our "Rule-Book" in essence, it defines what
/// constitutes a valid user.
/// </summary>
public partial class UserService
{
    // High-level validation entry point
    private void ValidateUserOnAdd(User user)
    {
        ValidateUserIsNotNull(user);

        Validate(
            (Rule: IsInvalid(user.Id), parameter: nameof(User.Id)),
            (Rule: IsInvalid(user.OrganizationId), parameter: nameof(User.OrganizationId)),
            (Rule: IsInvalid(user.FirstName), parameter: nameof(User.FirstName)),
            (Rule: IsInvalid(user.LastName), parameter: nameof(User.LastName)),
            (Rule: IsInvalid(user.Email), parameter: nameof(User.Email)),
            (Rule: IsInvalid(user.Phone), parameter: nameof(User.Phone)),
            (Rule: IsInvalid(user.Role), parameter: nameof(User.Role)),
            (Rule: IsInvalid(user.PasswordHash), parameter: nameof(User.PasswordHash)),
            (Rule: IsInvalid(user.RegistrationDate), parameter: nameof(User.RegistrationDate))
        );

        this.loggingBroker.LogInformation($"Successfully validated user: {user?.Email}");
    }

    // Checks the database to ensure a user with this ID doesn't already exist.
    private async ValueTask ValidateUserDoesNotAlreadyExistAsync(User user)
    {
        User? existingUser =
            await this.storageBroker.SelectUserByIdAsync(user.Id);

        if (existingUser is not null)
        {
            throw new AlreadyExistsUserException(user.Id);
        }
    }

    private static void ValidateUserIsNotNull(User user)
    {
        if (user is null)
        {
            throw new NullUserException();
        }
    }

    // Rule: Guid must not be empty
    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    // Rule: String must not be null, empty or whitespace
    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };

    // Rule: DateTime must not be default
    private static dynamic IsInvalid(DateTime date) => new
    {
        Condition = date == default,
        Message = "Date is required"
    };

    // The Standard "Validate" engine to collect all errors.
    private static void Validate(params (dynamic Rule, string parameter)[] validations)
    {
        var invalidUserException = new InvalidUserException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidUserException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }

            invalidUserException.ThrowIfContainsErrors();
        }
    }
}
