using AuthExample.Core.Abstraction.Errors;

namespace AuthExample.Core.Abstraction.Interfaces
{
    public interface IValidator
    {
        ValidationResult Validate(string fieldName, string value);
        ValidationResult ValidateAll(Dictionary<string, string> fieldsToValidate);
    }
}
