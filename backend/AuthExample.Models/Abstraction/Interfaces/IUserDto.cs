using AuthExample.Core.Abstraction.Enums;

namespace AuthExample.Core.Abstraction.Interfaces
{
    public interface IUserDto
    {
        string Email { get; set; }
        Guid Id { get; set; }
        List<RolesEnum> Roles { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}