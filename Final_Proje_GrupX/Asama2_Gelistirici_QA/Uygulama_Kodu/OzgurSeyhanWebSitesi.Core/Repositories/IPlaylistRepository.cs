using OzgurSeyhanWebSitesi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Repositories
{
    public interface IPlaylistRepository : IGenericRepository<Playlist>
    {
        /// <summary>
        /// PlaylistId'ye göre playlist getirir
        /// </summary>
        Task<Playlist?> GetByPlaylistIdAsync(string playlistId);

        /// <summary>
        /// Öğretmene ait tüm playlistleri getirir
        /// </summary>
        Task<IEnumerable<Playlist>> GetByOgretmenIdAsync(int ogretmenId);
    }
}
