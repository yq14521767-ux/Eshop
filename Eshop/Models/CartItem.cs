namespace Eshop.Models
{
    public class CartItem // 购物车项类
    {
        public int Id { get; set; } // 购物车项ID
        
        public int CartId { get; set; } // 关联购物车
        public Cart Cart { get; set; } // 购物车对象

        //外键：对应的商品
        public int ProductId { get; set; } // 商品ID
        public Product Product { get; set; } // 商品对象

        public int Quantity { get; set; } // 商品数量

    }
}
