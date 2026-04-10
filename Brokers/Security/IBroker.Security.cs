namespace Template.Api.Brokers.Security;

public interface ISecurityBroker
{
    string HashPassword(string password);

    bool VerifyPassWord(string password, string hash);
}