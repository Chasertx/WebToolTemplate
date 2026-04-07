namespace Template.Api.Brokers.Security;

public class SecurityBroker : ISecurityBroker
{
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password);
}
