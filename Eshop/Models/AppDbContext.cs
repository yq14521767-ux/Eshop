using Microsoft.EntityFrameworkCore;
namespace Eshop.Models
{
    public class AppDbContext : DbContext     // 应用程序数据库上下文 EF
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)  // 构造函数，接收配置选项
        {
        }

        public DbSet<Product> Products { get; set; }  // 产品集合，EF会自动创建对应的数据库表
        public DbSet<Cart> Carts { get; set; }  // 购物车集合
        public DbSet<CartItem> CartItems { get; set; }  // 购物车项集合
        public DbSet<Order> Orders { get; set; }  // 订单集合
        public DbSet<OrderItem> OrderItems { get; set; }  // 订单项集合
        public DbSet<User> Users { get; set; }  // 用户集合

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 把 Order.Status 枚举存成字符串
            builder.Entity<Order>()
                   .Property(o => o.Status)
                   .HasConversion<string>();
        }

    }
}
