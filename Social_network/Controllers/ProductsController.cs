using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social_network.Services;

namespace Social_network.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int? categoryId = null,
            [FromQuery] string? q = null,
            [FromQuery] decimal? priceFrom = null,
            [FromQuery] decimal? priceTo = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            [FromQuery] string sort = "created_at",
            [FromQuery] bool desc = false)
        {
            var (products, totalCount) = await _productService.GetProductsAsync(
                categoryId, q, priceFrom, priceTo, page, size, sort, desc);

            return Ok(new
            {
                Products = products,
                TotalCount = totalCount,
                Page = page,
                PageSize = size,
                TotalPages = (int)Math.Ceiling((double)totalCount / size)
            });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound("Продукт не найден");

            return Ok(product);
        }
    }
}
