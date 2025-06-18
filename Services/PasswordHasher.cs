using System.Security.Cryptography;
using System.Text;

namespace CVOIS.Services
{
    public class PasswordHasher
    {
        public static string HashPassword(string? password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedbytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));   //building bytes of array using UTF8

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedbytes.Length; i++)
                {
                    builder.Append(hashedbytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string EncryptPassword(string password)
        {
            if (password == null)
            {
                return null;
            }
            else
            {
                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedpassword = Convert.ToBase64String(storePassword);
                return encryptedpassword;
            }
        }

        public static string DecryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] encryptedpassword = Convert.FromBase64String(password);
                string decryptedpassword = ASCIIEncoding.ASCII.GetString(encryptedpassword);
                return decryptedpassword;
            }
        }
    }
}
