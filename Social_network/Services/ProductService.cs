using Social_network.Models.DTOs;
using Social_network.Models.Entities;
using Social_network.Repositories;
using Social_network.Controllers;

namespace Social_network.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<(IEnumerable<ProductResponse> Products, int TotalCount)> GetProductsAsync(
            int? categoryId = null,
            string? searchQuery = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            int page = 1,
            int pageSize = 20,
            string sortBy = "created_at",
            bool sortDesc = false)
        {
            var products = await _productRepository.GetProductsAsync(
                categoryId, searchQuery, priceFrom, priceTo, page, pageSize, sortBy, sortDesc);
            
            var totalCount = await _productRepository.GetProductsCountAsync(
                categoryId, searchQuery, priceFrom, priceTo);

            var productResponses = products.Select(MapToResponse);

            return (productResponses, totalCount);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return product != null ? MapToResponse(product) : null;
        }

        public async Task<ProductResponse?> CreateProductAsync(CreateProductRequest request)
        {
            // TODO: Implement create product logic
            return await Task.FromResult<ProductResponse?>(null);
        }

        public async Task<ProductResponse?> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            // TODO: Implement update product logic
            return await Task.FromResult<ProductResponse?>(null);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            // TODO: Implement delete product logic
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateInventoryAsync(int productId, int quantity)
        {
            // TODO: Implement update inventory logic
            return await Task.FromResult(true);
        }

        private ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Sku = product.Sku,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Currency = product.Currency,
                CategoryId = product.CategoryId,
                Active = product.Active,
                CreatedAt = product.CreatedAt,
                Quantity = product.Inventory?.Quantity ?? 0,
                Images = product.Images.Select(i => new ProductImageResponse
                {
                    Id = i.Id,
                    Url = i.Url,
                    SortOrder = i.SortOrder
                }).OrderBy(i => i.SortOrder).ToList(),
                Category = product.Category != null ? new CategoryResponse
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                    ParentId = product.Category.ParentId
                } : null
            };
        }
    }
}
