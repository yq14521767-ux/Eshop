using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "请输入用户名")]
        [StringLength(20, ErrorMessage = "用户名不可超过20个字符")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [ValidateNever]
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();

        [Required]
        [ValidateNever]
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

        [ValidateNever]
        public Cart Cart { get; set; } //表示用户有一个购物车

        public ICollection<Order> Orders { get; set; } = new List<Order>(); //表示用户可以有多个订单
        public bool IsAdmin { get; set; } = false; //表示用户是否为管理员
    }
}
