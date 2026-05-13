using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleNounDtos;
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
    public class FlowPuzzleNounService : GenericService<FlowPuzzleNoun>, IFlowPuzzleNounService
    {
        private readonly IFlowPuzzleNounRepository _flowPuzzleNounRepository;
        private readonly IMapper _mapper;

        public FlowPuzzleNounService(IGenericRepository<FlowPuzzleNoun> repository, IUnitOfWorks unitOfWorks, IFlowPuzzleNounRepository flowPuzzleNounRepository, IMapper mapper) 
            : base(repository, unitOfWorks)
        {
            _flowPuzzleNounRepository = flowPuzzleNounRepository;
            _mapper = mapper;
        }

        public async Task<List<FlowPuzzleNoun>> GetActiveNounsAsync()
        {
            return await _flowPuzzleNounRepository.GetActiveNounsAsync();
        }
    }
}
