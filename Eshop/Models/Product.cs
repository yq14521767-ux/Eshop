using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshop.Models
{
    public class Product  //产品
    {
        public int Id { get; set; }  // 主键，产品id

        [Required(ErrorMessage="请输入商品名称")]
        [StringLength(100,ErrorMessage="商品名称不可超过100个字符")]
        public string Name { get; set; }  //商品名称


        [StringLength(500,ErrorMessage ="描述不可超过500个字符")]
        public string Description { get; set; } = "";  // 商品描述

        [Required(ErrorMessage = "请输入商品价格")]
        [Range(0.01, 999999.99, ErrorMessage = "商品价格必须大于0")]
        [DataType(DataType.Currency)] // 指定数据类型为货币
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }   // 商品价格

        [Range(0, int.MaxValue, ErrorMessage = "库存不能为负数")]
        [Required(ErrorMessage ="请输入库存数量")]
        public int Stock { get; set; }  // 商品库存

        [Url(ErrorMessage = "请输入有效的图片地址")]
        public string ImageUrl { get; set; }  // 商品图片URL
    }
}
