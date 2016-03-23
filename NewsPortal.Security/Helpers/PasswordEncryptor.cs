using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NewsPortal.Security
{
    /// <summary>
    /// Шифровальщик паролей
    /// </summary>
    static class PasswordEncryptor
    {
        /// <summary>
        /// Получение хеша пароля
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(string password)
        {
            byte[] saltBytes = GenerateSalt();
            return GetHashedPaswordWithSalt(password, saltBytes);
        }

        /// <summary>
        /// Проверка соответствия пароля хешу
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool Validate(string password, string hash)
        {
            byte[] saltBytes = GetSalt(hash);
            return string.Equals(GetHashedPaswordWithSalt(password, saltBytes), hash);
        }

        private static byte[] GenerateSalt()
        {
            int saltLength = new Random().Next(4, 16);
            byte[] salt = new byte[saltLength];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static byte[] GetSalt(string hash)
        {
            byte[] hashAndSaltBytes = Convert.FromBase64String(hash);
            return hashAndSaltBytes.Skip(64).ToArray();
        }

        private static string GetHashedPaswordWithSalt(string password, byte[] saltBytes)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordAndSaltBytes = passwordBytes.Concat(saltBytes).ToArray();
            byte[] hashBytes;
            using (HashAlgorithm hashAlgorithm = new SHA512Managed())
            {
                hashBytes = hashAlgorithm.ComputeHash(passwordAndSaltBytes);
            }
            return Convert.ToBase64String(hashBytes.Concat(saltBytes).ToArray());
        }
    }
}
