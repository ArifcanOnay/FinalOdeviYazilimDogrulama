using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleSentenceDtos;
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
    public class FlowPuzzleSentenceService : GenericService<FlowPuzzleSentence>, IFlowPuzzleSentenceService
    {
        private readonly IFlowPuzzleSentenceRepository _sentenceRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorks _unitOfWorks;

        public FlowPuzzleSentenceService(
            IGenericRepository<FlowPuzzleSentence> repository,
            IUnitOfWorks unitOfWorks,
            IFlowPuzzleSentenceRepository sentenceRepository,
            IMapper mapper)
            : base(repository, unitOfWorks)
        {
            _sentenceRepository = sentenceRepository;
            _mapper = mapper;
            _unitOfWorks = unitOfWorks;
        }

        public async Task<List<FlowPuzzleSentenceDto>> GetActiveSentencesAsync()
        {
            var sentences = await _sentenceRepository.GetActiveSentencesWithNounsAsync();
            return _mapper.Map<List<FlowPuzzleSentenceDto>>(sentences);
        }

        public async Task<FlowPuzzleSentenceDto> CreateAsync(CreateFlowPuzzleSentenceDto dto)
        {
            var sentence = _mapper.Map<FlowPuzzleSentence>(dto);
            await _sentenceRepository.AddAsync(sentence);
            await _unitOfWorks.CommitAsync();

            var result = await _sentenceRepository.GetByIdAsync(sentence.Id);
            return _mapper.Map<FlowPuzzleSentenceDto>(result);
        }

        public async Task UpdateAsync(int id, UpdateFlowPuzzleSentenceDto dto)
        {
            var sentence = await _sentenceRepository.GetByIdAsync(id);
            if (sentence != null)
            {
                _mapper.Map(dto, sentence);
                _sentenceRepository.Update(sentence);
                await _unitOfWorks.CommitAsync();
            }
        }
    }
}
