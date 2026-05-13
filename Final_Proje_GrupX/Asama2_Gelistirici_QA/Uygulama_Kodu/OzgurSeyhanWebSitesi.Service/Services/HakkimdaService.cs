using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.HakkimdaDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;
using OzgurSeyhanWebSitesi.Bussinies.Services;

namespace OzgurSeyhanWebSitesi.Service.Services
{
    public class HakkimdaService : GenericService<Hakkimda>, IHakkimdaService
    {
        private readonly IHakkimdaRepository _hakkimdaRepository;
        private readonly IMapper _mapper;

        public HakkimdaService(IGenericRepository<Hakkimda> repository, IUnitOfWorks unitOfWorks, IHakkimdaRepository hakkimdaRepository, IMapper mapper) : base(repository, unitOfWorks)
        {
            _hakkimdaRepository = hakkimdaRepository;
            _mapper = mapper;
        }

        public async Task<HakkimdaDto?> GetHakkimdaAsync()
        {
            var hakkimda = await _hakkimdaRepository.GetFirstAsync();
            return hakkimda == null ? null : _mapper.Map<HakkimdaDto>(hakkimda);
        }

        public async Task<HakkimdaDto> UpdateHakkimdaAsync(UpdateHakkimdaDto dto, int? profilResmiId = null)
        {
            var hakkimda = await _hakkimdaRepository.GetFirstAsync();
            
            if (hakkimda == null)
            {
                // İlk kez oluşturuluyorsa
                hakkimda = _mapper.Map<Hakkimda>(dto);
                hakkimda.CreateDate = DateTime.Now;
                await _hakkimdaRepository.AddAsync(hakkimda);
            }
            else
            {
                // Güncelleme
                _mapper.Map(dto, hakkimda);
                hakkimda.UpdateDate = DateTime.Now;
                _hakkimdaRepository.Update(hakkimda);
            }
            
            await _unitOfWorks.CommitAsync();
            return _mapper.Map<HakkimdaDto>(hakkimda);
        }
    }
}
