using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace WebGSB.Models.Utilitaires
{
    public class MonMotPassHash
    {
        /// <summary>
        /// Hashes a password using SHA256
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hashed password as a byte array</returns>
        public static byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// Verifies a password against a stored hash
        /// </summary>
        /// <param name="inputPassword">Password input by the user</param>
        /// <param name="storedHash">Stored hashed password</param>
        /// <returns>True if passwords match, otherwise false</returns>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            var inputHash = HashPassword(inputPassword);
            var storedHashBytes = Convert.FromBase64String(storedHash);
            return CompareHashes(inputHash, storedHashBytes);
        }

        private static bool CompareHashes(byte[] inputHash, byte[] storedHash)
        {
            if (inputHash.Length != storedHash.Length)
                return false;

            for (int i = 0; i < inputHash.Length; i++)
            {
                if (inputHash[i] != storedHash[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Converts byte array to a base64 string
        /// </summary>
        public static string BytesToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }



}

