using Eshop.Filters;
using Eshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Eshop.Controllers.Admin
{
    [AdminFilter]
    public class AdminProductsController : Controller
    {
        public readonly AppDbContext _context;

        public AdminProductsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page,string search = null,int pageSize = 10)
        {
            int pageNum = (page ?? 1);
            if(page <  1) pageNum = 1;

            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search));
            }

            var totalCount = await products.CountAsync();

            var items = await products.OrderBy(p => p.Id)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedList = new StaticPagedList<Product>(items,pageNum,pageSize,totalCount);

            ViewBag.Search = search;
            return View(pagedList);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null) return NotFound();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product); //更新产品
                await _context.SaveChangesAsync(); //保存更改
                return RedirectToAction(nameof(Index)); //重定向到产品列表
            }
            return View(product); //如果模型状态无效，返回编辑视图
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
