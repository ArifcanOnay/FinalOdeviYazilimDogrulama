using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleLongPhraseDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class FlowPuzzleLongPhraseService : GenericService<FlowPuzzleLongPhrase>, IFlowPuzzleLongPhraseService
    {
        private readonly IFlowPuzzleLongPhraseRepository _longPhraseRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorks _unitOfWork;

        public FlowPuzzleLongPhraseService(
            IGenericRepository<FlowPuzzleLongPhrase> repository,
            IUnitOfWorks unitOfWorks,
            IFlowPuzzleLongPhraseRepository flowPuzzleLongPhraseRepository,
            IMapper mapper)
            : base(repository, unitOfWorks)
        {
            _longPhraseRepository = flowPuzzleLongPhraseRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWorks;
        }

        public async Task<List<FlowPuzzleLongPhraseDto>> GetActiveItemsAsync()
        {
            var items = await _longPhraseRepository.GetActiveItemsAsync();
            return _mapper.Map<List<FlowPuzzleLongPhraseDto>>(items);
        }

        public async Task<FlowPuzzleLongPhraseDto> CreateAsync(CreateFlowPuzzleLongPhraseDto dto)
        {
            var entity = _mapper.Map<FlowPuzzleLongPhrase>(dto);
            await _longPhraseRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            var created = await _longPhraseRepository.GetByIdAsync(entity.Id);
            return _mapper.Map<FlowPuzzleLongPhraseDto>(created);
        }

        public async Task UpdateAsync(int id, UpdateFlowPuzzleLongPhraseDto dto)
        {
            var entity = await _longPhraseRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return;
            }

            _mapper.Map(dto, entity);
            _longPhraseRepository.Update(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}
