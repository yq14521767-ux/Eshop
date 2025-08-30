using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models
{
    public class Order
    {
        public int Id { get; set; } // 订单ID
        public DateTime OrderDate { get; set; } // 订单日期
        public OrderStatus Status { get; set; } = OrderStatus.待付款;// 订单状态,待收货 / 已收货

        public int UserId { get; set; } // 外键：对应的用户ID
        public User User { get; set; } // 用户对象

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }// 订单总金额

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // 订单项列表
    }
    public class OrderItem
    {
        public int Id { get; set; } // 订单项ID

        public int ProductId { get; set; } // 商品ID
        public Product Product { get; set; } // 商品对象

        public int Quantity { get; set; } // 商品数量

        public int OrderId { get; set; } // 外键：对应的订单ID
        public Order Order { get; set; } // 订单对象

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } // 下单时的单价
    }
}
