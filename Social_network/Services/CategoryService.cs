using Social_network.Models.DTOs;
using Social_network.Models.Entities;
using Social_network.Repositories;
using Social_network.Controllers;

namespace Social_network.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(MapToResponse);
        }

        public async Task<CategoryResponse?> CreateCategoryAsync(CreateCategoryRequest request)
        {
            // TODO: Implement create category logic
            return await Task.FromResult<CategoryResponse?>(null);
        }

        public async Task<CategoryResponse?> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
        {
            // TODO: Implement update category logic
            return await Task.FromResult<CategoryResponse?>(null);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            // TODO: Implement delete category logic
            return await Task.FromResult(true);
        }

        private CategoryResponse MapToResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId
            };
        }
    }
}
