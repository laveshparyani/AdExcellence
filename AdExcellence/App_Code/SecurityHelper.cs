using System;
using System.Security.Cryptography;

namespace AdExcellence
{
    /// <summary>
    /// PBKDF2 password hashing helper. Stored format: "iterations.saltBase64.hashBase64".
    /// VerifyPassword also accepts legacy plain-text values so existing accounts keep working.
    /// </summary>
    public static class SecurityHelper
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        public static string HashPassword(string password)
        {
            if (password == null) password = string.Empty;
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] key = pbkdf2.GetBytes(KeySize);
                return Iterations + "." + Convert.ToBase64String(salt) + "." + Convert.ToBase64String(key);
            }
        }

        public static bool VerifyPassword(string password, string stored)
        {
            if (stored == null) return false;
            if (password == null) password = string.Empty;

            string[] parts = stored.Split('.');
            // Legacy plain-text fallback: stored values that are not in the hashed format
            // are compared directly so pre-existing accounts still work.
            if (parts.Length != 3)
            {
                return stored == password;
            }

            try
            {
                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expected = Convert.FromBase64String(parts[2]);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    byte[] actual = pbkdf2.GetBytes(expected.Length);
                    bool same = true;
                    for (int i = 0; i < expected.Length; i++)
                    {
                        if (expected[i] != actual[i]) same = false;
                    }
                    return same;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
