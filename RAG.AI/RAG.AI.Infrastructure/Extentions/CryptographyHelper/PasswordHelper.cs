using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace RAG.AI.Infrastructure.Extentions.CryptographyHelper;

public static class PasswordHelper
{
    public static byte[] GenerateSalt()
    {
        byte[] salt = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
    public static string GeneratePassword(byte[] salt, string password)
    {
        // Hash the password using the salt
        var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 32));
        return hashedPassword;
    }
}


