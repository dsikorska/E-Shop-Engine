using System.Security.Cryptography;
using System.Text;

namespace E_Shop_Engine.Utilities
{
    public static class SHA
    {
        public static string GetSHA256Hash(string sum)
        {
            SHA256Managed sha256 = new SHA256Managed();
            string resultString = null;

            byte[] byteConcat = Encoding.UTF8.GetBytes(sum);
            int byteNumber = Encoding.UTF8.GetByteCount(sum);

            byte[] result = sha256.ComputeHash(byteConcat, 0, byteNumber);

            foreach (byte a in result)
            {
                resultString += a.ToString("x2");
            }

            return resultString;
        }
    }
}
