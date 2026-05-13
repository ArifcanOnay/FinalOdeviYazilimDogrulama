using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleRelativeClauseDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class FlowPuzzleRelativeClauseService : GenericService<FlowPuzzleRelativeClause>, IFlowPuzzleRelativeClauseService
    {
        private readonly IFlowPuzzleRelativeClauseRepository _relativeClauseRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorks _unitOfWork;

        public FlowPuzzleRelativeClauseService(
            IGenericRepository<FlowPuzzleRelativeClause> repository,
            IUnitOfWorks unitOfWorks,
            IFlowPuzzleRelativeClauseRepository flowPuzzleRelativeClauseRepository,
            IMapper mapper)
            : base(repository, unitOfWorks)
        {
            _relativeClauseRepository = flowPuzzleRelativeClauseRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWorks;
        }

        public async Task<List<FlowPuzzleRelativeClauseDto>> GetActiveItemsAsync()
        {
            var items = await _relativeClauseRepository.GetActiveItemsAsync();
            return _mapper.Map<List<FlowPuzzleRelativeClauseDto>>(items);
        }

        public async Task<FlowPuzzleRelativeClauseDto> CreateAsync(CreateFlowPuzzleRelativeClauseDto dto)
        {
            var entity = _mapper.Map<FlowPuzzleRelativeClause>(dto);
            await _relativeClauseRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            var created = await _relativeClauseRepository.GetByIdAsync(entity.Id);
            return _mapper.Map<FlowPuzzleRelativeClauseDto>(created);
        }

        public async Task UpdateAsync(int id, UpdateFlowPuzzleRelativeClauseDto dto)
        {
            var entity = await _relativeClauseRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return;
            }

            _mapper.Map(dto, entity);
            _relativeClauseRepository.Update(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}
