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

        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var user = await _repo.GetUserByEmailAsync(request.Email);
            if (user == null) return null;

            var result = _hasher.VerifyHashedPassword(request.Email, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Success)
                return _tokenGen.Generate(user.Id, user.Email);

            return null;
        }

        public async Task<int> RegisterAsync(RegisterRequest req)
        {
            var hash = _hasher.HashPassword(req.Email, req.Password);
            var user = new User
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                BirthDate = req.BirthDate,
                Gender = req.Gender,
                Interests = req.Interests,
                City = req.City,
                Email = req.Email,
                PasswordHash = hash
            };
            return await _repo.InsertUserAsync(user);
        }

        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var u = await _repo.GetUserByIdAsync(id);
            if (u == null) return null;

            return new UserResponse
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                BirthDate = u.BirthDate,
                Gender = u.Gender,
                Interests = u.Interests,
                City = u.City,
                Email = u.Email
            };
        }

        public async Task<IEnumerable<User>> SearchAsync(string? firstName, string? lastName)
        {
            return await _repo.SearchUsersAsync(firstName, lastName);
        }
    }
}
