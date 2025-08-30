using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eshop.Filters
{
    public class AdminFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;


            var isAdmin = httpContext.Session.GetString("IsAdmin");

            if (isAdmin != "True")
            {
                // 如果不是管理员，重定向到登录页面或显示错误
                context.Result = new RedirectToActionResult("Login", "Users", null);
            }
        }
    }
}
