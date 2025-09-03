using Eshop.Filters;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers
{
    [LoginFilter]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        //显示购物车
        public async Task<IActionResult> Index(string? search)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId.Value };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            //搜索
            if (!string.IsNullOrEmpty(search))
            {
                cart.CartItems = cart.CartItems
                    .Where(ci => ci.Product !=null && ci.Product.Name.Contains(search))
                    .ToList();
            }

            ViewBag.Search = search;    
            return View(cart);

        }

        //加入购物车
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var product = await _context.Products.FindAsync(productId); // 查找产品
            if (product == null) return NotFound(); // 如果产品不存在，返回404未找到

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId.Value };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = Math.Max(1, quantity) // 比较，确保数量至少为1
                };
                cart.CartItems.Add(cartItem); // 如果购物车项不存在，则添加新的购物车项
            }
            else
            {
                cartItem.Quantity++; // 如果购物车项已存在，则数量加1
            }

            TempData["Success"] = "已加入购物车"; // 设置成功消息
            //ViewBag.Success = "已加入购物车"; 

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Products"); // 重定向到购物车页面
        }

        //从购物车中删除商品
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.Id == itemId && ci.Cart.UserId == userId); // 查找购物车项

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);// 如果购物车项存在，则删除
                await _context.SaveChangesAsync(); // 保存更改
            }

            return RedirectToAction("Index"); // 重定向到购物车页面
        }

    }
}
