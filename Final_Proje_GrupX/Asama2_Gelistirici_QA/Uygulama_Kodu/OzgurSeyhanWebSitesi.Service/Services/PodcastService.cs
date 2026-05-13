using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class PodcastService : GenericService<Podcast>,IPodcsatService
    {
        public PodcastService(IGenericRepository<Podcast> repository, IUnitOfWorks unitOfWorks) : base(repository, unitOfWorks)
        {
        }
    }
}
