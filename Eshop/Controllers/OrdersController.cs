using Eshop.Filters;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers
{
    [LoginFilter]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        //查看all订单
        public async Task<IActionResult> Index(string status,DateTime? startDate,DateTime? endDate)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var orders =  _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId.Value)
                .AsQueryable();

            //按状态筛选
            if (!string.IsNullOrEmpty(status) && status!= "All")
            {
                if(Enum.TryParse<OrderStatus>(status,out var parsedStatus))
                {
                    orders = orders.Where(o => o.Status == parsedStatus);
                }
            }
            //按时间筛选
            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate <= endDate.Value);
            }

            var ordersQ = await orders.OrderBy(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        //下单
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create()
        {
            var userId=HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Users");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId==userId.Value);

            // 检查购物车是否存在且不为空
            if (cart == null || !cart.CartItems.Any())
            {
                TempData["Error"] = "购物车为空！";
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order()
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                Status = OrderStatus.待付款
            };

            foreach (var ci in cart.CartItems)
            {
                if(ci.Product == null) continue;

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                });
                if (ci.Product != null)
                {
                    ci.Product.Stock = Math.Max(0, ci.Product.Stock - ci.Quantity);
                }
            }

            order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

            _context.Orders.Add(order);  // 添加订单到数据库

            _context.CartItems.RemoveRange(cart.CartItems);  // 清空已选择的购物车项

            await _context.SaveChangesAsync();  // 保存更改到数据库

            //return RedirectToAction("Details", new { id = order.Id }); // 重定向到订单详情
            return RedirectToAction("Index","Orders");
        }

        //查看订单详情
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Users");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId.Value);

            if (order == null) return NotFound(); // 如果订单不存在，返回404

            return View(order);
        }


        //确认收货
        public async Task<IActionResult> Confirm(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
            if(order == null) return NotFound();

            if(order.Status == OrderStatus.待收货 || order.Status == OrderStatus.待发货)
            {
                order.Status = OrderStatus.已收货; // 更新订单状态为已收货
                await _context.SaveChangesAsync(); // 保存更改到数据库
            }
            
            return RedirectToAction("Index");
        }

        //模拟付款
        public async Task<IActionResult> Pay(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
            if (order == null) return NotFound();

            if (order.Status == OrderStatus.待付款)
            {
                order.Status = OrderStatus.待发货; // 更新订单状态为已收货
                await _context.SaveChangesAsync(); // 保存更改到数据库
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteO(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}
