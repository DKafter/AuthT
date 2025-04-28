using AuthExample.Core.Abstraction.Enums;
using AuthExample.Core.Abstraction.Errors;
using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Models.Validation
{
    public class Validator : IValidator
    {
        private readonly Dictionary<string, Func<string, ValidationResult>> _validatorMethods;
        private readonly IErrorFactory _errorFactory;

        public Validator(IErrorFactory errorFactory)
        {
            _errorFactory = errorFactory ?? throw new ArgumentNullException(nameof(errorFactory));

            _validatorMethods = new Dictionary<string, Func<string, ValidationResult>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Username", ValidateUsername },
                { "Email", ValidateEmail },
                { "Password", ValidatePassword }
            };
        }

        public ValidationResult Validate(string fieldName, string value)
        {
            if (string.IsNullOrEmpty(fieldName))
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_FIELD_NAME));

            if (_validatorMethods.TryGetValue(fieldName, out var validatorMethod))
            {
                return validatorMethod(value);
            }

            return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.UNKNOWN_FIELD));
        }

        public ValidationResult ValidateAll(Dictionary<string, string> fieldsToValidate)
        {
            var errors = new List<IErrorObject>();

            foreach (var field in fieldsToValidate)
            {
                var result = Validate(field.Key, field.Value);
                if (!result.IsValid)
                {
                    errors.Add(result.Error);
                }
            }

            return errors.Any()
                ? ValidationResult.Failure(errors.ToArray())
                : ValidationResult.Success();
        }

        private ValidationResult ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_NULL_USERNAME));

            if (username.Length < (int)UserConstEnum.MIN_USERNAME_LENGTH)
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_MIN_USERNAME_LENGTH));

            if (username.Length > (int)UserConstEnum.MAX_USERNAME_LENGTH)
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_MAX_USERNAME_LENGTH));

            return ValidationResult.Success();
        }

        private ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_NULL_EMAIL));

            // Можно добавить более сложную проверку формата email с помощью регулярного выражения
            if (!email.Contains("@") || !email.Contains("."))
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_EMAIL_FORMAT));

            return ValidationResult.Success();
        }

        private ValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_NULL_PASSWORD));

            if (password.Length < (int)UserConstEnum.MIN_PASSWORD_LENGTH)
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_MIN_PASSWORD_LENGTH));

            // Можно добавить проверку сложности пароля
            bool hasUpperCase = password.Any(char.IsUpper);
            bool hasLowerCase = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecialChar = password.Any(c => !char.IsLetterOrDigit(c));

            if (!(hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar))
                return ValidationResult.Failure(_errorFactory.Create(ErrorNamesEnum.INVALID_PASSWORD_COMPLEXITY));

            return ValidationResult.Success();
        }
    }
}
