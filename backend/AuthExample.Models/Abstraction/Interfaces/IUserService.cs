using AuthExample.Application.Data;
using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Application.Services
{
    public interface IUserService
    {
        Task<IUserResponse> AuthenticateUserAsync(string username, string password);
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        Task<IUserResponse> CreateUserAsync(string username, string email, string password);
        Task<IUserDto> GetUserByIdAsync(Guid userId);
        Task<(bool Success, string Error)> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken, Guid userId);
    }
}