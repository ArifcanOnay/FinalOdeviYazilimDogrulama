using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class ReferansLinkService : GenericService<ReferansLink>, IReferansLinkService
    {
        private readonly IReferansLinkRepository _referansLinkRepository;
        private readonly IMapper _mapper;

        public ReferansLinkService(IReferansLinkRepository referansLinkRepository, IUnitOfWorks unitOfWorks, IMapper mapper) 
            : base(referansLinkRepository, unitOfWorks)
        {
            _referansLinkRepository = referansLinkRepository;
            _mapper = mapper;
        }

        public List<ReferansLinkDto> GetAllOrderedBySira()
        {
            var links = _referansLinkRepository.GetAllOrderedBySira();
            return _mapper.Map<List<ReferansLinkDto>>(links);
        }
    }
}
