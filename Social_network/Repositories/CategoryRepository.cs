using Dapper;
using Social_network.Models.Entities;
using Social_network.Models.DTOs;

namespace Social_network.Repositories
{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(IConfiguration config) 
            : base(config.GetConnectionString("DefaultConnection")!)
        {
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using var conn = CreateConnection();
            var categories = await conn.QueryAsync<Category>(@"
                SELECT id, name, parent_id AS ParentId
                FROM categories
                ORDER BY name
            ");

            return BuildCategoryTree(categories.ToList());
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            using var conn = CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Category>(@"
                SELECT id, name, parent_id AS ParentId
                FROM categories
                WHERE id = @id
            ", new { id });
        }

        private List<Category> BuildCategoryTree(List<Category> categories)
        {
            var categoryDict = categories.ToDictionary(c => c.Id);
            var rootCategories = new List<Category>();

            foreach (var category in categories)
            {
                if (category.ParentId.HasValue && categoryDict.TryGetValue(category.ParentId.Value, out var parent))
                {
                    parent.Children.Add(category);
                }
                else
                {
                    rootCategories.Add(category);
                }
            }

            return rootCategories;
        }
    }
}
