using System.Security.Cryptography;
using System.Text;

namespace Vanfist.Utils;

public class PasswordUtil
{
    public static string Encode(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    public static bool Verify(string password, string hashedPassword)
    {
        var hashedInput = Encode(password);
        return hashedInput == hashedPassword;
    }
}