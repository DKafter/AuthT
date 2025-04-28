using AuthExample.Core.Abstract.Errors;
using AuthExample.Core.Abstraction.Enums;
using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Core.Abstraction.Errors
{
    public class ErrorFactory : IErrorFactory
    {
        private readonly Dictionary<ErrorNamesEnum, Func<IErrorObject>> _errorCreators;

        public ErrorFactory()
        {
            _errorCreators = new Dictionary<ErrorNamesEnum, Func<IErrorObject>>
            {
                { ErrorNamesEnum.INVALID_FIELD_NAME, () => new InvalidFieldNameError() },
                { ErrorNamesEnum.UNKNOWN_FIELD, () => new UnknownFieldError() },
                { ErrorNamesEnum.INVALID_NULL_USERNAME, () => new InvalidNullUsernameError() },
                { ErrorNamesEnum.INVALID_MIN_USERNAME_LENGTH, () => new InvalidMinUsernameLengthError() },
                { ErrorNamesEnum.INVALID_MAX_USERNAME_LENGTH, () => new InvalidMaxUsernameLengthError() },
                { ErrorNamesEnum.INVALID_NULL_EMAIL, () => new InvalidNullEmailError() },
                { ErrorNamesEnum.INVALID_EMAIL_FORMAT, () => new InvalidEmailFormatError() },
                { ErrorNamesEnum.INVALID_NULL_PASSWORD, () => new InvalidNullPasswordError() },
                { ErrorNamesEnum.INVALID_MIN_PASSWORD_LENGTH, () => new InvalidMinPasswordLengthError() },
                { ErrorNamesEnum.INVALID_PASSWORD_COMPLEXITY, () => new InvalidPasswordComplexityError() }
            };
        }

        public IErrorObject Create(ErrorNamesEnum errorName)
        {
            if (_errorCreators.TryGetValue(errorName, out var creator))
            {
                return creator();
            }

            throw new ArgumentException($"Unknown error name: {errorName}");
        }
    }
}
