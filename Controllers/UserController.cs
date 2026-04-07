using Microsoft.AspNetCore.Mvc;
using Template.Api.Models.Foundation.User;
using Template.Api.Models.Foundation.User.Exceptions;
using Template.Api.Services.Foundations.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;

    /// <summary>
    /// User controller constructor injects
    /// and sets the userService class.
    /// </summary>
    /// <param name="userService"></param>
    public UsersController(IUserService userService) =>
        this.userService = userService;

    /// <summary>
    /// POST api/users inserts a new user
    /// record into the database using the broker.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost]
    public async ValueTask<ActionResult<User>> PostUserAsync([FromBody] User user)
    {
        try
        {
            User addedUser =
                await this.userService.AddUserAsync(user);

            return CreatedAtAction(nameof(GetUserById), new { id = addedUser.Id }, addedUser);
        }
        catch (UserValidationException userValidationException)
        {
            return Conflict(userValidationException.Message);
        }
    }

    /// <summary>
    /// Gets a list of all users in the user table.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<IQueryable<User>> GetAllUsers()
    {
        IQueryable<User> users =
            this.userService.RetrieveAllUsers();

        return Ok(users);
    }

    /// <summary>
    /// Temporary place holder for retrieving a user
    /// by it's unique id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public ActionResult<User> GetUserById(Guid id)
    {
        return Ok();
    }
}
