using Template.Api.Models.Foundation.User;

namespace Template.Api.Services.Foundations.Users;

public interface IUserService
{
    ValueTask<User> AddUserAsync(User user);
    ValueTask<User?> RetrieveUserByIdAsync(Guid userId);
    IQueryable<User> RetrieveAllUsers();
}
