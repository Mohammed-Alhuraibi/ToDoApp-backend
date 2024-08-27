using System;
using System.Security.Cryptography;

namespace ToDo.Services
{
    public static class CodeGenerator
    {
        public static string GenerateCode()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[6];
                rng.GetBytes(data);
                int value = BitConverter.ToInt32(data, 0) % 1000000;
                value = Math.Abs(value); // Ensure a positive 6-digit number

                return value.ToString("D6"); // Format as a 6-digit string
            }
        }
    }
}
