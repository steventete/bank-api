using System.Security.Cryptography;
using System.Text;

namespace Utilidades
{
    public static class SeguridadUtilidades
    {
        public static string ObtenerHash(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
