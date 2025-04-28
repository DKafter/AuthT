using AuthExample.Core.Abstract.Errors;
using AuthExample.Core.Abstraction.Enums;

namespace AuthExample.Core.Abstraction.Interfaces
{
    public interface IErrorObject
    {
        ErrorNamesEnum ErrorName { get; }
        string Message { get; }

        (ErrorNamesEnum, string) GetError();
    }
}