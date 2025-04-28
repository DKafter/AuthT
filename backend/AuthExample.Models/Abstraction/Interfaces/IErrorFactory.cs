using AuthExample.Core.Abstraction.Enums;

namespace AuthExample.Core.Abstraction.Interfaces
{
    public interface IErrorFactory
    {
        IErrorObject Create(ErrorNamesEnum errorName);
    }
}
