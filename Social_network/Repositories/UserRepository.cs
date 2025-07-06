using Dapper;
using Npgsql;
using Social_network.Models.Entities;

namespace Social_network.Repositories
{
    public class UserRepository
    {
        private readonly string _connString;

        public UserRepository(IConfiguration config)
        {
            _connString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var conn = new NpgsqlConnection(_connString);
            return await conn.QueryFirstOrDefaultAsync<User>(@"
                SELECT 
                    id,
                    first_name AS ""FirstName"",
                    last_name AS ""LastName"",
                    birth_date AS ""BirthDate"",
                    gender,
                    interests,
                    city,
                    email,
                    password_hash AS ""PasswordHash""
                FROM users
                WHERE email = @email
            ", new { email });
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connString);
            return await conn.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM users WHERE id = @id",
                new { id }
            );
        }

        public async Task<int> InsertUserAsync(User user)
        {
            using var conn = new NpgsqlConnection(_connString);
            var sql = @"
            INSERT INTO users
            (first_name, last_name, birth_date, gender, interests, city, email, password_hash)
            VALUES
            (@FirstName, @LastName, @BirthDate, @Gender, @Interests, @City, @Email, @PasswordHash)
            RETURNING id;
        ";
            return await conn.ExecuteScalarAsync<int>(sql, user);
        }
    }
}
