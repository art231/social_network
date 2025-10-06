using Microsoft.AspNetCore.Identity;
using Social_network.Models.DTOs;
using Social_network.Models.Entities;
using Social_network.Repositories;
using Social_network.Utils;

namespace Social_network.Services
{
    public class UserService
    {
        private readonly UserRepository _repo;
        private readonly JwtTokenGenerator _tokenGen;
        private readonly PasswordHasher<string> _hasher;

        public UserService(UserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _tokenGen = new JwtTokenGenerator(config);
            _hasher = new PasswordHasher<string>();
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var user = await _repo.GetUserByEmailAsync(request.Email);
            if (user == null) return null;

            var result = _hasher.VerifyHashedPassword(request.Email, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Success)
            {
                var accessToken = _tokenGen.Generate(user.Id, user.Email);
                var refreshToken = _tokenGen.GenerateRefreshToken();
                
                return new AuthResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = new UserResponse
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        CreatedAt = user.CreatedAt,
                        Roles = user.Roles.Select(r => r.Name).ToList()
                    }
                };
            }

            return null;
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest req)
        {
            var existingUser = await _repo.GetUserByEmailAsync(req.Email);
            if (existingUser != null)
                return null;

            var hash = _hasher.HashPassword(req.Email, req.Password);
            var user = new User
            {
                Name = req.Name,
                Email = req.Email,
                PasswordHash = hash,
                CreatedAt = DateTime.UtcNow
            };
            
            var userId = await _repo.InsertUserAsync(user);
            
            var accessToken = _tokenGen.Generate(userId, user.Email);
            var refreshToken = _tokenGen.GenerateRefreshToken();
            
            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = new UserResponse
                {
                    Id = userId,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Roles = new List<string> { "user" }
                }
            };
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var user = await _repo.GetUserByIdAsync(id);
            if (user == null) return null;

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };
        }

        public async Task<UserResponse?> GetUserByEmailAsync(string email)
        {
            var user = await _repo.GetUserByEmailAsync(email);
            if (user == null) return null;

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };
        }

        public async Task<UserResponse?> UpdateUserAsync(int id, RegisterRequest request)
        {
            var user = await _repo.GetUserByIdAsync(id);
            if (user == null) return null;

            user.Name = request.Name;
            user.Email = request.Email;

            var updated = await _repo.UpdateUserAsync(user);
            if (!updated)
                return null;

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };
        }
    }
}
