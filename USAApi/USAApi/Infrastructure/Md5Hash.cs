using System.Security.Cryptography;
using System.Text;

namespace USAApi.Infrastructure
{
    public class Md5Hash
    {
        public static string ForString(string input)
        {
            using(var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                var builder = new StringBuilder();
                for(int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
