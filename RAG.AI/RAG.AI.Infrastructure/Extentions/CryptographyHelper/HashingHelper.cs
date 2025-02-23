using System.Security.Cryptography;
using System.Text;

namespace RAG.AI.Infrastructure.Extentions.CryptographyHelper;

public class HashingHelper
{
    public static string Sha2(string input)
    {
        using (SHA256 sha2 = new SHA256Managed())
        {
            var hash = sha2.ComputeHash(Encoding.Default.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
    public static string GetBase64EncodedSHA2Hash(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            return Convert.ToBase64String(sha256Hash.ComputeHash(Encoding.Default.GetBytes(input)));
        }
    }
    public static string LoopHash(string input, int loopCount)
    {
        for (int i = 0; i < loopCount; i++)
        {
            input = GetBase64EncodedSHA2Hash(input);
        }

        return input;
    }
}


