using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleSentenceDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IFlowPuzzleSentenceService : IGenericService<FlowPuzzleSentence>
    {
        Task<List<FlowPuzzleSentenceDto>> GetActiveSentencesAsync();
        Task<FlowPuzzleSentenceDto> CreateAsync(CreateFlowPuzzleSentenceDto dto);
        Task UpdateAsync(int id, UpdateFlowPuzzleSentenceDto dto);
    }
}
