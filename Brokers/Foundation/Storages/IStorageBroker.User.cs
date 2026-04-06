using Template.Api.Models.Foundation.User;
public partial interface IStorageBroker
{
    ValueTask<User> InsertUserAsync(User user);
    ValueTask<User?> SelectUserByIdAsync(Guid userId);
    IQueryable<User> SelectAllUsers();
}