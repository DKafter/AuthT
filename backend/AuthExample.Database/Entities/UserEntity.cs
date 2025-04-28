using AuthExample.Core.Abstraction.Enums;

namespace AuthExample.Database.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public List<RolesEnum> Roles { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public UserEntity()
        {}
        public void AddRole(RolesEnum role)
        {
            if(!Roles.Contains(role)) Roles.Add(role);
        }
        public void RemoveRole(RolesEnum role)
        {
            if (Roles.Contains(role)) Roles.Remove(role);   
        }
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
        public void UpdateLastLogin() => UpdateAt = DateTime.UtcNow;
    }
}
