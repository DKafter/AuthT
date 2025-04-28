using AuthExample.Core.Abstraction.Enums;
using AuthExample.Core.Abstraction.Interfaces;

namespace AuthExample.Application.Data.Dtos
{
    public class UserDto : IUserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<RolesEnum> Roles { get; set; }
        public UserDto()
        {
            Roles = new List<RolesEnum>();
        }
    }
}
