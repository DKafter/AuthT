using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Application.Data
{
    public interface IUserResponse
    {
        IErrorObject? Error { get; set; }
        string Message { get; set; }
        bool Success { get; set; }
        IUserDto? User { get; set; }
        string? Token { get; set; }
        string? RefreshToken { get; set; }
    }
}