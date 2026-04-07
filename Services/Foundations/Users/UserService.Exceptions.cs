using Microsoft.EntityFrameworkCore;
using Template.Api.Models.Foundation.User;
using Template.Api.Models.Foundation.User.Exceptions;

namespace Template.Api.Services.Foundations.Users;

// Delegate class for handling exceptions
// in the UserService.cs file.
public partial class UserService
{
    private delegate ValueTask<User?> ReturningUserFunction();
    private delegate IQueryable<User> ReturningUsersFunction();

    // Exception wrapper for single user operations.
    private async ValueTask<User?> TryCatch(ReturningUserFunction returningUserFunction)
    {
        try
        {
            User? user = await returningUserFunction();

            this.loggingBroker.LogInformation($"Successfully persisted user: {user?.Email}");
            return user;
        }
        catch (NullUserException nullUserException)
        {
            this.loggingBroker.LogError(nullUserException);
            throw new UserValidationException(nullUserException);
        }
        catch (InvalidUserException invalidUserException)
        {
            this.loggingBroker.LogError(invalidUserException);
            throw new UserValidationException(invalidUserException);
        }
        catch (AlreadyExistsUserException alreadyExistsUserException)
        {
            this.loggingBroker.LogError(alreadyExistsUserException);
            throw new UserValidationException(alreadyExistsUserException);
        }
        catch (DbUpdateException dbUpdateException)
        {
            this.loggingBroker.LogError(dbUpdateException);
            throw new Exception("Database error occurred.", dbUpdateException);
        }
        catch (Exception exception)
        {
            this.loggingBroker.LogError(exception);
            throw new Exception("Service error occurred.", exception);
        }
    }

    // Exception wrapper for multiple user operations.
    private IQueryable<User> TryCatch(ReturningUsersFunction returningUsersFunction)
    {
        try
        {
            return returningUsersFunction();
        }
        catch (Exception exception)
        {
            this.loggingBroker.LogError(exception);
            throw new Exception("Failed to retrieve users.", exception);
        }
    }
}
