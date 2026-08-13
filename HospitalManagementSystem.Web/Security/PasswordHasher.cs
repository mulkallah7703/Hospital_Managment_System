using System;
using System.Security.Cryptography;

namespace HospitalManagementSystem.Web.Security
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static void CreateHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] saltBytes = new byte[SaltSize];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            byte[] hashBytes;
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                hashBytes = pbkdf2.GetBytes(HashSize);
            }

            passwordSalt = Convert.ToBase64String(saltBytes);
            passwordHash = Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            if (string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(passwordHash)
                || string.IsNullOrEmpty(passwordSalt))
            {
                return false;
            }

            byte[] saltBytes = Convert.FromBase64String(passwordSalt);
            byte[] expectedHash = Convert.FromBase64String(passwordHash);

            byte[] actualHash;
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                actualHash = pbkdf2.GetBytes(HashSize);
            }

            if (expectedHash.Length != actualHash.Length)
            {
                return false;
            }

            int result = 0;
            for (int i = 0; i < expectedHash.Length; i++)
            {
                result |= expectedHash[i] ^ actualHash[i];
            }

            return result == 0;
        }
    }
}
