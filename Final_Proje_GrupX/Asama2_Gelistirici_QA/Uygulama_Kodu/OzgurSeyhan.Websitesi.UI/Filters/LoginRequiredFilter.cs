using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OzgurSeyhan.Websitesi.UI.Filters
{
    public class LoginRequiredFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userEmail = context.HttpContext.Session.GetString("UserEmail");

            if (userId == null || string.IsNullOrEmpty(userEmail))
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }
        }
    }

    public class LoginRequiredAttribute : TypeFilterAttribute
    {
        public LoginRequiredAttribute() : base(typeof(LoginRequiredFilter))
        {
        }
    }
}
