using Eshop.Models;
using Eshop.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersController> _logger;
        

        public UsersController(AppDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //注册
        //用于处理get
        public IActionResult Register()
        {
            return View();
        }
        //用于处理post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            

            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
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

                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "用户注册失败，用户名: {UserName}", user.UserName);
                    ModelState.AddModelError(string.Empty, $"注册失败：{ex.Message}");
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
                        _logger.LogWarning("注册校验失败 字段: {Field}, 错误: {Error}", kv.Key, err.ErrorMessage);
                    }
                }
                //ModelState.AddModelError(string.Empty, "请修正表单中的错误后再提交。");
            }
            return View(user);
        }

        //登录
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username,string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if(user == null || !PasswordHelper.VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
            {
                ModelState.AddModelError("", "用户名或密码错误");
                return View();
            }

            //临时存放用户id在Session中
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

            return RedirectToAction("Index", "Home");
        }

        //退出登录
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // 清除所有会话数据
            return RedirectToAction("Login");
        }

    }
}
