namespace AuthExample.Database.Entities
{
    public class RefreshTokenEntity
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public bool IsUsed { get; set; } = false;
        public string JwtId { get; set; }
        public virtual UserEntity User { get; set; }
        public RefreshTokenEntity() { }
        public RefreshTokenEntity(Guid guid, string token, Guid userId, DateTime issuedAt, DateTime expiresAt, string jwtId)
        {
            Id = guid;
            Token = token;
            UserId = userId;
            IssuedAt = issuedAt;
            ExpiresAt = expiresAt;
            JwtId = jwtId;
        }
        public void MarkAsUsed() => IsUsed = true;
        public void MarkAsRevoked() => IsRevoked = true;
        public bool IsActivate => !IsRevoked && !IsUsed && ExpiresAt > DateTime.UtcNow;
    }
}
