using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Services;

namespace TomasosPizzeria.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IDishService _dishService;

        public OrderController(IDishService dishService)
        {
            _dishService = dishService;
        }


        public async Task<IActionResult> Create()
        {
            var model = await _dishService.GetAllDishesAsync();
            return View(model);
        }
    }
}