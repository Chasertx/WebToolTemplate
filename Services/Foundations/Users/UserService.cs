using Template.Api.Models.Foundation.User;
using Microsoft.EntityFrameworkCore;
using Template.Api.Brokers.Logging;
using Template.Api.Brokers.Security;

namespace Template.Api.Services.Foundations.Users;

/// <summary>
/// This is the "Brain" of user operations, it
/// handles the sequence of the events.
/// </summary>
public partial class UserService : IUserService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;
    private readonly ISecurityBroker securityBroker;

    /// <summary>
    /// Injecting and setting the storage broker.
    /// </summary>
    /// <param name="storageBroker"></param>
    /// <param name="loggingBroker"></param>
    /// <param name="securityBroker"></param>
    public UserService(
        IStorageBroker storageBroker,
        ILoggingBroker loggingBroker,
        ISecurityBroker securityBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
        this.securityBroker = securityBroker;
    }

    /// <summary>
    /// Calls the broker service to persist a
    /// user to the database.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async ValueTask<User> AddUserAsync(User user)
    {
        return (await TryCatch(async () =>
        {
            ValidateUserOnAdd(user);
            await ValidateUserDoesNotAlreadyExistAsync(user);

            user.PasswordHash = this.securityBroker.HashPassword(user.PasswordHash);

            return await this.storageBroker.InsertUserAsync(user);
        }))!;
    }

    /// <summary>
    /// Retrieves a specific user according
    /// to it's userId.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public ValueTask<User?> RetrieveUserByIdAsync(Guid userId) =>
    TryCatch(async () =>
    {
        return await this.storageBroker.SelectUserByIdAsync(userId);
    });

    /// <summary>
    /// Retrieves a specific user by email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public ValueTask<User?> RetrieveUserByEmailAsync(string email) =>
    TryCatch(async () =>
    {
        return await this.storageBroker.SelectUserByEmailAsync(email);
    });

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns></returns>
    public IQueryable<User> RetrieveAllUsers() =>
    TryCatch(() =>
    {
        return this.storageBroker.SelectAllUsers();
    });
}
