using Microsoft.AspNetCore.Mvc;

namespace OzgurSeyhan.Websitesi.UI.ViewComponents.LayoutComponents
{
    public class _LayoutHeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
