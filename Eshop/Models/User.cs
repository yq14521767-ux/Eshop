using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "请输入用户名")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "用户名长度不能少于6位")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "用户名只能包含字母和数字")]
        public string UserName { get; set; } = string.Empty; //设置一个初始的空字符串值，避免属性默认值为 null

        [Required(ErrorMessage = "请输入密码")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20位之间")]
        [DataType(DataType.Password)]//标记属性为密码类型
        [NotMapped] // 不存到数据库
        public string Password { get; set; } = string.Empty;

        [Required]
        [ValidateNever]  //不验证该属性
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
