using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace Btf.Utilities.Hash
{
    public class PasswordHash : IPasswordHash
    {
        private byte[] salt;

        public PasswordHash()
        {
            salt = new byte[128 / 8];

            for (int i = 0; i < salt.Length; ++i)
                salt[i] = (byte)i;
        }

        public string GetHash(string password)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
