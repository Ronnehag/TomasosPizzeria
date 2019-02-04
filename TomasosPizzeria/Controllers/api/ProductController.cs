using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers.api
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IDishService _dishService;

        public ProductController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var product = await _dishService.GetDishAsync(id);
            return Ok(product);
        }
    }
}