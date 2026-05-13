using Microsoft.AspNetCore.Mvc;

namespace OzgurSeyhan.Websitesi.UI.ViewComponents.DashboardComponents
{
    public class StatisticsCardsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int videoCount, int playlistCount, int ozelDersCount, int podcastCount)
        {
            var model = new StatisticsViewModel
            {
                VideoCount = videoCount,
                PlaylistCount = playlistCount,
                OzelDersCount = ozelDersCount,
                PodcastCount = podcastCount
            };

            return View(model);
        }
    }

    public class StatisticsViewModel
    {
        public int VideoCount { get; set; }
        public int PlaylistCount { get; set; }
        public int OzelDersCount { get; set; }
        public int PodcastCount { get; set; }
    }
}
