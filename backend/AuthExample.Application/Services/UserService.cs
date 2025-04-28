using AuthExample.Application.Data;
using AuthExample.Application.Data.Dtos;
using AuthExample.Core.Abstraction.Enums;
using AuthExample.Core.Abstraction.Interfaces;
using AuthExample.Database.Data;
using AuthExample.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthExample.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator _validator;
        private readonly IConfiguration _configuration;
        public UserService(AppDbContext context, IPasswordHasher passwordHasher, IValidator validator, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _validator = validator;
            _configuration = configuration;
        }
        public async Task<IUserResponse> CreateUserAsync(string username, string email, string password)
        {
            var fieldsToValidate = new Dictionary<string, string>
            {
                { "Username", username },
                { "Email", email },
                { "Password", password }
            };

            var validationResult = _validator.ValidateAll(fieldsToValidate);
            //if (!validationResult.IsValid)
            //{
            //    return (false, validationResult.Error.GetError().Item2);
            //}

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username.Contains(username) || u.Email.Contains(email));
            if (existingUser != null)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = "User with this username or email already exists.",
                    User = null,
                    Error = validationResult.Error
                };
            }

            var (passwordHash, salt) = _passwordHasher.HashPassword(password);

            var newUser = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = passwordHash,
                Salt = salt,
                CreateAt = DateTime.UtcNow,
                IsActive = true,
                UpdateAt = null,
                Roles = new List<RolesEnum> { RolesEnum.USER }
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            var newUserDto = new UserDto
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
                Roles = newUser.Roles,            
            };
            return new UserResponse
            {
                Success = true,
                Message = "User created successfully.",
                User = newUserDto,
                Error = validationResult.Error,
                Token = null,
                RefreshToken = null
            };
        }
        public async Task<IUserResponse> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Contains(email));
            var fieldsToValidate = new Dictionary<string, string>
            {
                { "Username", "NULL" },
                { "Email", email },
                { "Password", password }
            };

            var validationResult = _validator.ValidateAll(fieldsToValidate);

            if (user == null || !user.IsActive)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = "Invalid username or password.",
                    User = null,
                    Error = validationResult.Error
                };
            }

            var (passwordHash, salt) = _passwordHasher.HashPassword(password);
            if (!_passwordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                return new UserResponse
                {
                    Success = false,
                    Message = "Invalid username or password.",
                    User = null,
                    Error = validationResult.Error
                };
            }

            user.UpdateLastLogin();
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles,
                Id = user.Id            };

            return new UserResponse
            {
                Success = true,
                Message = "User authenticated successfully.",
                User = userDto,
                RefreshToken = refreshToken.Token,
                Token = token,
            };
        }
        public async Task<(bool Success, string Error)> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenEntity = GetPrincipalFromExpiredToken(refreshToken);
            if (string.IsNullOrEmpty(refreshToken))
            {
                return (false, "Неверный токен");
            }
            var userId = Guid.Parse(refreshTokenEntity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId);
            if (storedRefreshToken == null || storedRefreshToken.Token != refreshToken || storedRefreshToken.ExpiresAt < DateTime.UtcNow
                || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
            {
                return (false, "Неверный токен");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.IsActive)
            {
                return (false, "Пользователь не найден");
            }
            storedRefreshToken.IsUsed = true;
            var newToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user.Id);
            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();

            return (true, newToken);
        }
        public async Task<bool> RevokeTokenAsync(string refreshToken, Guid userId)
        {
            var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken && t.UserId == userId);
            if (storedRefreshToken == null || storedRefreshToken.IsUsed || storedRefreshToken.IsRevoked)
            {
                return false;
            }

            storedRefreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IUserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            return new UserDto
            {
                Id = userId,
                Username = user.Username,
                Email = user.Email,
                Roles = user.Roles
            };
        }
        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.IsActive)
            {
                return false;
            }
            if (!_passwordHasher.VerifyPassword(currentPassword, user.PasswordHash, user.Salt))
            {
                return false;
            }
            var validationResult = _validator.Validate("Password", newPassword);
            if (validationResult.IsValid)
            {
                return false;
            }

            var (passwordHash, salt) = _passwordHasher.HashPassword(newPassword);
            user.PasswordHash = passwordHash;
            user.Salt = salt;
            await _context.SaveChangesAsync();

            return true;
        }
        private string GenerateJwtToken(UserEntity user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // Добавление ролей в claims
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role.ToString(), role.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15), // Время жизни токена
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private RefreshTokenEntity GenerateRefreshToken(Guid userId)
        {
            var randomBytes = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                Token = Convert.ToBase64String(randomBytes),
                UserId = userId,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7), // Refresh токен действителен 7 дней
                JwtId = Guid.NewGuid().ToString()
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false // Не проверяем срок действия токена
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
    }
}
