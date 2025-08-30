using Eshop.Filters;
using Eshop.Models;
using Eshop.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers.Admin
{
    [AdminFilter]
    public class AdminDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardVm
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TotalSales = await _context.Orders
                    .Where(o => o.Status != OrderStatus.已取消)
                    .SumAsync(o => o.TotalAmount)
            };

            return View(vm);

        }

    }
}
