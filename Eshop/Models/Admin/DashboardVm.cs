namespace Eshop.Models.Admin
{
    public class DashboardVm
    {
        public int TotalProducts { get; set; } // 总产品数
        public int TotalOrders { get; set; }   // 总订单数
        public int TotalUsers { get; set; }    // 总用户数
        public decimal TotalSales { get; set; } // 总销售额
    }
}
