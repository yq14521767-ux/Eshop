using Eshop.Filters;
using Eshop.Models;
using Eshop.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Eshop.Controllers.Admin
{
    [AdminFilter]
    public class AdminUsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminUsersController> _logger;

        public AdminUsersController(AppDbContext context, ILogger<AdminUsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? page,string? search,int pageSize = 10)
        {
            var pageNum = (page ?? 1);
            if(pageNum < 1) pageNum = 1;

            var users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.UserName.Contains(search));
            }

            var totalCount = await users.CountAsync();

            var items = await users.OrderBy( u => u.Id)
                .Skip((pageNum-1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var usersList = new StaticPagedList<User>(items,pageNum,pageSize,totalCount);

            ViewBag.Search =search;
            return View(usersList);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                //生成密码哈希 + 盐
                PasswordHelper.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password)) //检查密码是否为空或输入的空字符
            {
                ModelState.AddModelError("password", "请输入密码");
            }

            if (ModelState.IsValid)
            {
                if(await _context.Users.AnyAsync(u=> u.UserName == user.UserName))
                {
                    ModelState.AddModelError("UserName", "用户名已存在");
                    return View(user);
                }

                try
                {
                    //生成密码哈希 + 盐
                    PasswordHelper.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "用户添加失败，用户名: {UserName}", user.UserName);
                    ModelState.AddModelError(string.Empty, $"添加失败：{ex.Message}");
                    return View(user);
                }

            }
            else
            {
                // 记录所有模型验证错误
                foreach (var kv in ModelState)
                {
                    foreach (var err in kv.Value.Errors)
                    {
                        _logger.LogWarning("添加校验失败 字段: {Field}, 错误: {Error}", kv.Key, err.ErrorMessage);
                    }
                }
                ModelState.AddModelError(string.Empty, "请修正表单中的错误后再提交。");
            }
            return View(user);
        }

    }
}
