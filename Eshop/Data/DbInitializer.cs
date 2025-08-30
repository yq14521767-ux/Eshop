using Eshop.Models;

namespace Eshop.Data
{
    public class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            //如果数据库已经有商品，就不在重复添加
            //if (context.Products.Any()) return;

            //var products = new List<Product>
            //{
            //    new Product { Name="iPhone 15", Price=5999, Stock=10, Description="苹果最新手机" },
            //    new Product { Name="Nike 球鞋", Price=699, Stock=50, Description="耐克运动鞋" },
            //    new Product { Name="Dell XPS 13", Price=8999, Stock=5, Description="轻薄笔记本" }
            //};

            //context.Products.AddRange(products); // 添加商品到数据库上下文（仅是添加，未保存）
            //context.SaveChanges(); // 保存更改到数据库

            //if (context.Users.Any()) return; // 如果数据库已经有用户，就不在重复添加

            //var users = new List<User>
            //    {
            //        new User { UserName="admin", Password="admin",IsAdmin=true}
            //    };
            //context.Users.AddRange(users); // 添加商品到数据库上下文（仅是添加，未保存）
            //context.SaveChanges(); // 保存更改到数据库
        }
    }
}
