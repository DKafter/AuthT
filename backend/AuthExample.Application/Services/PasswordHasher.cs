using AuthExample.Core.Abstraction.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace AuthExample.Application.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public (string PasswordHash, string Salt) HashPassword(string password)
        {
            // Генерация случайной соли
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Хеширование пароля с солью
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Возвращаем хеш и соль в виде строк Base64
            return (hashedPassword, Convert.ToBase64String(salt));
        }
        public bool VerifyPassword(string password, string passwordHash, string salt)
        {
            // Преобразование соли из строки Base64 в массив байтов
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Хеширование введенного пароля с той же солью
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Сравнение хешей
            return hashedPassword == passwordHash;
        }
    }
}

