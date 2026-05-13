using Microsoft.AspNetCore.Mvc;

namespace OzgurSeyhan.Websitesi.UI.ViewComponents.DashboardComponents
{
    public class QuickAccessViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
