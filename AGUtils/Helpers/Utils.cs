using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Static.Helpers
{
    public static class Utils
    {
        public static byte[] ConvertNonPrimitiveToBytes<T>(T obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            var serializedObj = obj.GetType() == typeof(string)
                    ? JsonConvert.SerializeObject(obj)
                    : obj.ToString();

            return Encoding.UTF8.GetBytes(serializedObj);
        }

        public static string GenerateHash(string data)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
