using Dapper;
using Social_network.Models.Entities;

namespace Social_network.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration config) 
            : base(config.GetConnectionString("DefaultConnection")!)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(@"
                SELECT 
                    id,
                    name,
                    email,
                    password_hash AS ""PasswordHash"",
                    created_at AS ""CreatedAt""
                FROM users
                WHERE email = @email
            ", new { email });
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(@"
                SELECT 
                    id,
                    name,
                    email,
                    password_hash AS ""PasswordHash"",
                    created_at AS ""CreatedAt""
                FROM users
                WHERE id = @id
            ", new { id });
        }

        public async Task<int> InsertUserAsync(User user)
        {
            using var conn = CreateConnection();
            var sql = @"
            INSERT INTO users
            (name, email, password_hash, created_at)
            VALUES
            (@Name, @Email, @PasswordHash, @CreatedAt)
            RETURNING id;
        ";
            return await conn.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string? firstNamePrefix, string? lastNamePrefix)
        {
            using var conn = CreateConnection();

            var query = @"
                SELECT 
                    id,
                    name,
                    email,
                    password_hash AS ""PasswordHash"",
                    created_at AS ""CreatedAt""
                FROM users 
                WHERE name ILIKE @NamePattern
                ORDER BY id
                LIMIT 100;";

            return await conn.QueryAsync<User>(query, new
            {
                NamePattern = $"%{firstNamePrefix}%"
            });
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            using var conn = CreateConnection();
            var sql = @"
                UPDATE users 
                SET name = @Name, email = @Email
                WHERE id = @Id";
            
            var affectedRows = await conn.ExecuteAsync(sql, user);
            return affectedRows > 0;
        }
    }
}
