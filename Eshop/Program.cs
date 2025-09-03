using Microsoft.EntityFrameworkCore;
//using Eshop.Data;
//using Eshop.Models;  // 引入数据初始化类

namespace Eshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 添加MVC
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<Eshop.Models.AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                ));

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2); // 设置会话超时时间
                options.Cookie.HttpOnly = true; // 设置Cookie为HttpOnly，防止客户端脚本访问
                options.Cookie.IsEssential = true; // 确保Cookie在GDPR合规性下仍然可用
                options.Cookie.SameSite = SameSiteMode.Lax; // 设置SameSite属性，防止CSRF攻击
            });
            builder.Services.AddHttpContextAccessor(); // 注入HttpContextAccessor


            var app = builder.Build();

            // 开发环境下显示详细错误信息
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // 启用会话中间件

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //// 迁移并初始化数据库
            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    context.Database.Migrate();
            //    DbInitializer.Initialize(context);  // 调用数据初始化方法
            //}

            app.Run();
        }
    }
}
