using AuthExample.Core.Abstraction.Enums;
using AuthExample.Core.Abstraction.Errors;
using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Core.Models
{
    public class UserModel
    {
        public Guid Id { get; }
        public string Username { get; }
        public string Email { get; }
        public string PasswordHash { get; }
        public string Salt { get; }
        public List<RolesEnum> Roles { get; }
        public bool IsActive { get; }
        public DateTime CreateAt { get; }
        public DateTime? UpdateAt { get; }
        private UserModel(
            Guid id,
            string username,
            string email,
            string passwordHash,
            string salt,
            DateTime createAt
            )
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Salt = salt;
            Roles = new List<RolesEnum>();
            CreateAt = DateTime.UtcNow;
            IsActive = true;
        }
        public static(UserModel user, ValidationResult ValidationResult) Create(
            Guid id,
            string username,
            string email,
            string password,
            IPasswordHasher passwordHasher,
            IValidator validator)
        {
            var fieldsToValidate = new Dictionary<string, string>
            {
                { "Username", username },
                { "Email", email },
                { "Password", password }
            };

            var validationResult = validator.ValidateAll(fieldsToValidate);
            if (!validationResult.IsValid)
            {
                return (null, validationResult);
            }

            var (passwordHash, salt) = passwordHasher.HashPassword(password);
            var user = new UserModel(id, username, email, passwordHash, salt, DateTime.UtcNow);
            return (user,  validationResult);
        }
    }
}
