using Microsoft.AspNetCore.Mvc;
using Eshop.Models;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers
{
    public class ProductsController : Controller  //产品控制器
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)  // 构造函数，接收数据库上下文
        {
            _context = context;
        }

        //商品列表
        public async Task<IActionResult> Index()  // 异步方法，返回商品列表视图
        {
            var products = await _context.Products
                .OrderByDescending(p => p.Id)
                .ToListAsync();// 从数据库获取所有产品
            return View(products);  // 返回视图并传递产品列表
        }

        //商品详情
        public async Task<IActionResult> Details(int id)
        {
            if (id == null) return BadRequest(); // 如果id为空，返回错误请求

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);  // 根据id查询产品

            if(product==null) return NotFound();  // 如果产品不存在，返回404未找到
            return View(product);  // 返回产品详情视图

        }

    }
}
