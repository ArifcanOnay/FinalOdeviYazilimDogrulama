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
    public class OgretmenService : GenericService<Ogretmen>,IOgretmenService
    {
        public OgretmenService(IGenericRepository<Ogretmen> repository, IUnitOfWorks unitOfWorks) : base(repository, unitOfWorks)
        {
        }
    }
}
