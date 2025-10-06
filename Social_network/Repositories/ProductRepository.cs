using Dapper;
using Social_network.Models.Entities;
using Social_network.Models.DTOs;

namespace Social_network.Repositories
{
    public class ProductRepository : BaseRepository
    {
        public ProductRepository(IConfiguration config) 
            : base(config.GetConnectionString("DefaultConnection")!)
        {
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(
            int? categoryId = null,
            string? searchQuery = null,
            decimal? priceFrom = null,
            decimal? priceTo = null,
            int page = 1,
            int pageSize = 20,
            string sortBy = "created_at",
            bool sortDesc = false)
        {
            using var conn = CreateConnection();
            
            var whereConditions = new List<string> { "p.active = true" };
            var parameters = new DynamicParameters();

            if (categoryId.HasValue)
            {
                whereConditions.Add("p.category_id = @CategoryId");
                parameters.Add("CategoryId", categoryId.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                whereConditions.Add(@"(p.title ILIKE @SearchQuery OR p.description ILIKE @SearchQuery)");
                parameters.Add("SearchQuery", $"%{searchQuery}%");
            }

            if (priceFrom.HasValue)
            {
                whereConditions.Add("p.price >= @PriceFrom");
                parameters.Add("PriceFrom", priceFrom.Value);
            }

            if (priceTo.HasValue)
            {
                whereConditions.Add("p.price <= @PriceTo");
                parameters.Add("PriceTo", priceTo.Value);
            }

            var whereClause = whereConditions.Any() ? $"WHERE {string.Join(" AND ", whereConditions)}" : "";

            var orderBy = sortBy.ToLower() switch
            {
                "price" => "p.price",
                "title" => "p.title",
                _ => "p.created_at"
            };
            var orderDirection = sortDesc ? "DESC" : "ASC";

            var sql = $@"
                SELECT 
                    p.id, p.sku, p.title, p.description, p.price, p.currency,
                    p.category_id AS CategoryId, p.active, p.created_at AS CreatedAt,
                    c.id, c.name, c.parent_id AS ParentId,
                    i.product_id AS ProductId, i.quantity,
                    pi.id, pi.product_id AS ProductId, pi.url, pi.sort_order AS SortOrder
                FROM products p
                LEFT JOIN categories c ON p.category_id = c.id
                LEFT JOIN inventory i ON p.id = i.product_id
                LEFT JOIN product_images pi ON p.id = pi.product_id
                {whereClause}
                ORDER BY {orderBy} {orderDirection}
                LIMIT @PageSize OFFSET @Offset
            ";

            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", (page - 1) * pageSize);

            var productDict = new Dictionary<int, Product>();
            
            await conn.QueryAsync<Product, Category, Inventory, ProductImage, Product>(
                sql,
                (product, category, inventory, image) =>
                {
                    if (!productDict.TryGetValue(product.Id, out var productEntry))
                    {
                        productEntry = product;
                        productEntry.Category = category;
                        productEntry.Inventory = inventory;
                        productEntry.Images = new List<ProductImage>();
                        productDict.Add(product.Id, productEntry);
                    }

                    if (image != null && !productEntry.Images.Any(i => i.Id == image.Id))
                    {
                        productEntry.Images.Add(image);
                    }

                    return productEntry;
                },
                parameters,
                splitOn: "Id,ProductId,Id"
            );

            return productDict.Values;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using var conn = CreateConnection();
            
            var sql = @"
                SELECT 
                    p.id, p.sku, p.title, p.description, p.price, p.currency,
                    p.category_id AS CategoryId, p.active, p.created_at AS CreatedAt,
                    c.id, c.name, c.parent_id AS ParentId,
                    i.product_id AS ProductId, i.quantity,
                    pi.id, pi.product_id AS ProductId, pi.url, pi.sort_order AS SortOrder
                FROM products p
                LEFT JOIN categories c ON p.category_id = c.id
                LEFT JOIN inventory i ON p.id = i.product_id
                LEFT JOIN product_images pi ON p.id = pi.product_id
                WHERE p.id = @Id
            ";

            Product? product = null;
            
            await conn.QueryAsync<Product, Category, Inventory, ProductImage, Product>(
                sql,
                (p, category, inventory, image) =>
                {
                    product ??= p;
                    product.Category = category;
                    product.Inventory = inventory;
                    
                    if (image != null)
                    {
                        product.Images ??= new List<ProductImage>();
                        if (!product.Images.Any(i => i.Id == image.Id))
                        {
                            product.Images.Add(image);
                        }
                    }
                    
                    return product;
                },
                new { Id = id },
                splitOn: "Id,ProductId,Id"
            );

            return product;
        }

        public async Task<int> GetProductsCountAsync(
            int? categoryId = null,
            string? searchQuery = null,
            decimal? priceFrom = null,
            decimal? priceTo = null)
        {
            using var conn = CreateConnection();
            
            var whereConditions = new List<string> { "active = true" };
            var parameters = new DynamicParameters();

            if (categoryId.HasValue)
            {
                whereConditions.Add("category_id = @CategoryId");
                parameters.Add("CategoryId", categoryId.Value);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                whereConditions.Add(@"(title ILIKE @SearchQuery OR description ILIKE @SearchQuery)");
                parameters.Add("SearchQuery", $"%{searchQuery}%");
            }

            if (priceFrom.HasValue)
            {
                whereConditions.Add("price >= @PriceFrom");
                parameters.Add("PriceFrom", priceFrom.Value);
            }

            if (priceTo.HasValue)
            {
                whereConditions.Add("price <= @PriceTo");
                parameters.Add("PriceTo", priceTo.Value);
            }

            var whereClause = whereConditions.Any() ? $"WHERE {string.Join(" AND ", whereConditions)}" : "";

            var sql = $"SELECT COUNT(*) FROM products {whereClause}";
            
            return await conn.ExecuteScalarAsync<int>(sql, parameters);
        }
    }
}
