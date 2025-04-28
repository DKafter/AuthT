using AuthExample.Application.Data.Dtos;
using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Application.Data
{
    public class UserResponse : IUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IErrorObject? Error { get; set; }
        public IUserDto? User { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
