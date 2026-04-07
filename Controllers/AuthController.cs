using Microsoft.AspNetCore.Mvc;
using Template.Api.Brokers.Security;
using Template.Api.Services.Foundations.Users;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ISecurityBroker securityBroker;

    /// <summary>
    /// Auth controller constructor injects
    /// the user service and security broker.
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="securityBroker"></param>
    public AuthController(
        IUserService userService,
        ISecurityBroker securityBroker)
    {
        this.userService = userService;
        this.securityBroker = securityBroker;
    }

    /// <summary>
    /// POST api/auth/login authenticates a user
    /// and returns a JWT token.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async ValueTask<ActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await this.userService
            .RetrieveUserByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        bool isValidPassword = this.securityBroker
            .VerifyPassword(request.Password, user.PasswordHash);

        if (!isValidPassword)
        {
            return Unauthorized("Invalid email or password.");
        }

        string token = this.securityBroker
            .GenerateToken(user.Id.ToString(), user.Email, user.Role);

        return Ok(new { Token = token });
    }
}

/// <summary>
/// Request model for the login endpoint.
/// </summary>
public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
