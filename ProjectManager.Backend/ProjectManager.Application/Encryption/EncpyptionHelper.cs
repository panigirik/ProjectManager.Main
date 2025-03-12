using System.Text.RegularExpressions;
using CryptSharp.Utility;
using Microsoft.Extensions.Configuration;

namespace ProjectManager.Application.Encryption
{
    public class BlowfishEncryptionHelper : CryptSharp.Crypter
    {
        private readonly string _encryptionKey;

        public BlowfishEncryptionHelper(IConfiguration configuration)
        {
            _encryptionKey = configuration["EncryptionKey"];
        }

        public override int MaxKeyLength => 56;
        public override int MinKeyLength => 1;

        public override string GenerateSalt()
        {
            return GenerateSalt(10);
        }

        public override string GenerateSalt(int cost)
        {
            if (cost < 4 || cost > 31)
                throw new ArgumentException("Cost should be between 4 and 31.");

            byte[] salt = new byte[16];
            new Random().NextBytes(salt);
            char[] saltBase64 = UnixBase64.Encode(salt);
            return string.Format("$2a${0}${1}{2}", cost.ToString("00"), saltBase64, saltBase64);
        }

        public override string Crypt(byte[] key, string salt)
        {
            CheckKey(key);

            if (string.IsNullOrEmpty(salt))
                throw new ArgumentException("Salt cannot be null or empty.", nameof(salt));

            Match match = Regex.Match(salt, @"^\$2a\$(\d{2})\$(.{22})(.{31})$");
            if (!match.Success)
                throw new ArgumentException("Invalid salt format.", nameof(salt));

            int cost = int.Parse(match.Groups[1].Value);
            if (cost < 4 || cost > 31)
                throw new ArgumentException("Invalid cost value in salt.", nameof(salt));

            byte[] saltBytes = UnixBase64.Decode(match.Groups[2].Value, 128);

            bool keyResized = false;
            if (key.Length < MaxKeyLength)
            {
                Array.Resize(ref key, MaxKeyLength);
                keyResized = true;
            }

            byte[] hash = BlowfishCipher.BCrypt(key, saltBytes, cost);

            string result = string.Format("$2a${0}${1}{2}",
                cost.ToString("00"),
                new string(UnixBase64.Encode(saltBytes)),
                new string(UnixBase64.Encode(hash)));

            Array.Clear(hash, 0, hash.Length);
            Array.Clear(saltBytes, 0, saltBytes.Length);

            if (keyResized)
                Array.Clear(key, 0, key.Length);

            return result;
        }

        private void CheckKey(byte[] key)
        {
            if (key == null || key.Length == 0)
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        }
    }
}
