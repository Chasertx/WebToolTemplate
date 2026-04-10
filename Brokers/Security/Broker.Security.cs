using System.Security.Cryptography.X509Certificates;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Template.Api.Brokers.Security;

public class SecurityBroker : ISecurityBroker
{
    public string HashPassword(string password) =>
        BCryptNet.HashPassword(password);

    public bool VerifyPassWord(string password, string hash) =>
        BCryptNet.Verify(password, hash);
}