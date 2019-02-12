using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomasosPizzeria.Models.ViewModels;

namespace TomasosPizzeria.Controllers
{
    public class DishController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [ActionName("EditDish")]
        [Route("dish/save/{id}")]
        public IActionResult EditDish(EditDishViewModel mdl)
        {
            return null;
        }
    }
}