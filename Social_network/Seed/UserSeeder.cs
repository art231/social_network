using Bogus;
using Dapper;
using Npgsql;
using Social_network.Models.Entities;

namespace Social_network.Seed
{
    public class UserSeeder
    {
        private readonly string _connStr;

        public UserSeeder(string connStr)
        {
            _connStr = connStr;
        }

        public async Task GenerateAsync(int count = 1_000)
        {
            var faker = new Faker("en");
            var users = new List<User>(capacity: 10_000);

            using var conn = new NpgsqlConnection(_connStr);
            await conn.OpenAsync();

            for (int i = 0; i < count; i++)
            {
                var firstName = faker.Name.FirstName();
                var lastName = faker.Name.LastName();

                var user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = faker.Date.Between(new DateTime(1970, 1, 1), new DateTime(2005, 12, 31)),
                    Gender = faker.PickRandom("Male", "Female"),
                    Interests = faker.Lorem.Sentence(),
                    City = faker.Address.City(),
                    Email = $"user{i}@example.com", // ✅ гарантированно уникальный
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") // используем BCrypt
                };

                users.Add(user);

                // Вставка пачками по 10_000
                if (users.Count >= 10_000)
                {
                    await BulkInsertAsync(conn, users);
                    users.Clear();
                }
            }

            if (users.Count > 0)
            {
                await BulkInsertAsync(conn, users);
            }
        }

        private static async Task BulkInsertAsync(NpgsqlConnection conn, List<User> users)
        {
            const string sql = @"
            INSERT INTO users 
            (first_name, last_name, birth_date, gender, interests, city, email, password_hash)
            VALUES 
            (@FirstName, @LastName, @BirthDate, @Gender, @Interests, @City, @Email, @PasswordHash);";

            await conn.ExecuteAsync(sql, users);
        }
    }
}
