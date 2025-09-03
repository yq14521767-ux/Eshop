using Microsoft.AspNetCore.Mvc;
using Eshop.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Eshop.Controllers
{
    public class ProductsController : Controller 
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)  // 构造函数，接收数据库上下文
        {
            _context = context;
        }

        //商品列表
        public async Task<IActionResult> Index(int? page,string search = null,int pageSize = 8)  
        {
            int pageNum = (page ?? 1); //page若为空，就默认第1页
            if(pageNum < 1) pageNum = 1;

            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            var totalCount = await products.CountAsync();//总数

            //获取本页数据，根据id排序
            var items = await products.OrderBy(p => p.Id)
                .Skip((pageNum -1) * pageSize)  //跳过前面页得数据
                .Take(pageSize)  //只获取8条数据
                .ToListAsync();  //异步查询并将结果转为列表

            //使用StaticPagedList构建 IPagedList
            var pagedList = new StaticPagedList<Product>(items, pageNum, pageSize, totalCount);

            ViewBag.Search = search;
            return View(pagedList);
        }

        //商品详情
        public async Task<IActionResult> Details(int id,int? page,string search)
        {
            if (id == null) return BadRequest(); // 如果id为空，返回错误请求

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);  // 根据id查询产品

            if(product==null) return NotFound();  // 如果产品不存在，返回404未找到

            ViewBag.CurrentPage = page;
            ViewBag.Search = search;
            return View(product);  // 返回产品详情视图

        }

    }
}
