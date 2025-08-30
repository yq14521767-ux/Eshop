using System.ComponentModel.DataAnnotations;

namespace Eshop.Models
{
    public class Cart // 购物车类
    {
        public int Id { get; set; } // 购物车ID

        [Required]
        public int UserId { get; set; } // 用户ID

        public User User { get; set; } // 关联的用户

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>(); // 购物车项列表
    }
}
