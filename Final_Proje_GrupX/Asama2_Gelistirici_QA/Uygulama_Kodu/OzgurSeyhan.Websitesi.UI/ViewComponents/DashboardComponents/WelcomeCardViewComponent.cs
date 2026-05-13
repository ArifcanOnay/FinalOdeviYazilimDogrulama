using Microsoft.AspNetCore.Mvc;

namespace OzgurSeyhan.Websitesi.UI.ViewComponents.DashboardComponents
{
    public class WelcomeCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
