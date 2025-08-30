namespace Eshop.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>(); //存储分页结果中的当前页数据
        public int CurrentPage { get; set; }  //当前页
        public int TotalPages { get; set; }  //总页数
        public string? Search {  get; set; }  //搜索条件

    }
}
