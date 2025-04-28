using AuthExample.Core.Abstraction.Enums;
using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Core.Abstract.Errors
{
    public abstract class ErrorObject : IErrorObject
    {
        public abstract ErrorNamesEnum ErrorName { get; }
        public abstract string Message { get; }

        public (ErrorNamesEnum, string) GetError()
        {
            return (ErrorName, Message);
        }
    }
    public class InvalidFieldNameError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_FIELD_NAME;
        public override string Message => "Invalid field name";
    }

    public class UnknownFieldError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.UNKNOWN_FIELD;
        public override string Message => "Unknown field";
    }

    public class InvalidNullUsernameError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_NULL_USERNAME;
        public override string Message => "Username cannot be empty";
    }

    public class InvalidMinUsernameLengthError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_MIN_USERNAME_LENGTH;
        public override string Message => "Username is too short";
    }

    public class InvalidMaxUsernameLengthError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_MAX_USERNAME_LENGTH;
        public override string Message => "Username is too long";
    }

    public class InvalidNullEmailError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_NULL_EMAIL;
        public override string Message => "Email cannot be empty";
    }

    public class InvalidEmailFormatError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_EMAIL_FORMAT;
        public override string Message => "Invalid email format";
    }

    public class InvalidNullPasswordError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_NULL_PASSWORD;
        public override string Message => "Password cannot be empty";
    }

    public class InvalidMinPasswordLengthError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_MIN_PASSWORD_LENGTH;
        public override string Message => "Password is too short";
    }

    public class InvalidPasswordComplexityError : ErrorObject
    {
        public override ErrorNamesEnum ErrorName => ErrorNamesEnum.INVALID_PASSWORD_COMPLEXITY;
        public override string Message => "Password must contain uppercase, lowercase, digit and special character";
    }
}
