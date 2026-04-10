namespace Template.Api.Brokers.Security;

public interface ISecurityBroker
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
    string GenerateToken(string userId, string email, string role);
}
