using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Core.Abstraction.Errors
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public IErrorObject Error { get; }
        public IErrorObject[] Errors { get; }

        private ValidationResult(bool isValid, IErrorObject error = null, IErrorObject[] errors = null)
        {
            IsValid = isValid;
            Error = error;
            Errors = errors ?? Array.Empty<IErrorObject>();
        }

        public static ValidationResult Success() => new ValidationResult(true);

        public static ValidationResult Failure(IErrorObject error) =>
            new ValidationResult(false, error);

        public static ValidationResult Failure(IErrorObject[] errors) =>
            new ValidationResult(false, errors: errors);
    }
}
