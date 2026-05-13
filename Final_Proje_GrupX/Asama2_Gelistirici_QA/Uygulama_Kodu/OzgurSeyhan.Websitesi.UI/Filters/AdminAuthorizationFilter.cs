using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OzgurSeyhan.Websitesi.UI.Filters
{
    /// <summary>
    /// Admin paneline erişim kontrolü için authorization filter
    /// </summary>
    public class AdminAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Session'dan kullanıcı bilgilerini kontrol et
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userEmail = context.HttpContext.Session.GetString("UserEmail");

            // Kullanıcı giriş yapmamışsa login sayfasına yönlendir
            if (userId == null || string.IsNullOrEmpty(userEmail))
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
                return;
            }

            // Admin kontrolü - Session'da IsAdmin bilgisi var mı?
            var isAdmin = context.HttpContext.Session.GetString("IsAdmin");
            
            if (isAdmin != "true")
            {
                // Admin değilse yetkisiz erişim sayfasına yönlendir
                context.Result = new RedirectToActionResult("Index", "Home", null);
                return;
            }
        }

        // Gelecekte kullanılabilir
        private bool IsAdmin(string email)
        {
            // Admin email listesi veya database kontrolü
            var adminEmails = new[] { "admin@ozgurseyhan.com", "info@ozgurseyhan.com" };
            return adminEmails.Contains(email, StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Admin controller'lara uygulanacak attribute
    /// </summary>
    public class AdminAuthorizeAttribute : TypeFilterAttribute
    {
        public AdminAuthorizeAttribute() : base(typeof(AdminAuthorizationFilter))
        {
        }
    }
}
