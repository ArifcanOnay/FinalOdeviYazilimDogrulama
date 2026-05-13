using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.VideoProgressDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class VideoProgressService : IVideoProgressService
    {
        private readonly IVideoProgressRepository _progressRepository;
        private readonly IGenericRepository<VideoProgress> _repository;
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;

        public VideoProgressService(
            IGenericRepository<VideoProgress> repository,
            IUnitOfWorks unitOfWorks,
            IMapper mapper,
            IVideoProgressRepository progressRepository)
        {
            _repository = repository;
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _progressRepository = progressRepository;
        }

        public async Task<VideoProgressDto> GetProgressAsync(int userId, string videoId)
        {
            var progress = await _progressRepository.GetProgressAsync(userId, videoId);
            return _mapper.Map<VideoProgressDto>(progress);
        }

        public async Task<List<VideoProgressDto>> GetUserPlaylistProgressAsync(int userId, int playlistId)
        {
            var progresses = await _progressRepository.GetUserPlaylistProgressAsync(userId, playlistId);
            return _mapper.Map<List<VideoProgressDto>>(progresses);
        }

        public async Task<List<VideoProgressDto>> GetAllUserProgressAsync(int userId)
        {
            var progresses = await _progressRepository.GetAllUserProgressAsync(userId);
            return _mapper.Map<List<VideoProgressDto>>(progresses);
        }

        public async Task<VideoProgressDto> UpdateProgressAsync(UpdateVideoProgressDto updateDto)
        {
            var existingProgress = await _progressRepository.GetProgressAsync(updateDto.UserId, updateDto.VideoId);

            if (existingProgress == null)
            {
                // Yeni kayıt oluştur
                var newProgress = new VideoProgress
                {
                    UserId = updateDto.UserId,
                    PlaylistId = updateDto.PlaylistId,
                    VideoId = updateDto.VideoId,
                    IzlenmeYuzdesi = updateDto.IzlenmeYuzdesi,
                    IzlenenSaniye = updateDto.IzlenenSaniye,
                    ToplamSure = updateDto.ToplamSure,
                    TamamlandiMi = updateDto.IzlenmeYuzdesi >= 95,
                    IlkIzlemeTarihi = DateTime.UtcNow,
                    SonIzlemeTarihi = DateTime.UtcNow
                };

                await _repository.AddAsync(newProgress);
                await _unitOfWorks.CommitAsync();

                return _mapper.Map<VideoProgressDto>(newProgress);
            }
            else
            {
                // Mevcut kaydı güncelle
                existingProgress.IzlenmeYuzdesi = updateDto.IzlenmeYuzdesi;
                existingProgress.IzlenenSaniye = updateDto.IzlenenSaniye;
                existingProgress.ToplamSure = updateDto.ToplamSure;
                existingProgress.TamamlandiMi = updateDto.IzlenmeYuzdesi >= 95;
                existingProgress.SonIzlemeTarihi = DateTime.UtcNow;

                await _progressRepository.UpdateProgressAsync(existingProgress);

                return _mapper.Map<VideoProgressDto>(existingProgress);
            }
        }

        public async Task<Dictionary<string, decimal>> GetPlaylistProgressSummaryAsync(int userId, int playlistId)
        {
            var progresses = await _progressRepository.GetUserPlaylistProgressAsync(userId, playlistId);

            var summary = new Dictionary<string, decimal>
            {
                ["totalVideos"] = progresses.Count,
                ["completedVideos"] = progresses.Count(p => p.TamamlandiMi),
                ["averageProgress"] = progresses.Any() ? progresses.Average(p => p.IzlenmeYuzdesi) : 0,
                ["totalWatchedSeconds"] = progresses.Sum(p => p.IzlenenSaniye)
            };

            return summary;
        }

        // IGenericService implementations
        public async Task AddAsync(VideoProgressDto entity)
        {
            var progress = _mapper.Map<VideoProgress>(entity);
            await _repository.AddAsync(progress);
            await _unitOfWorks.CommitAsync();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _unitOfWorks.Commit();
        }

        public List<VideoProgressDto> GetAll()
        {
            var progresses = _repository.GetAll();
            return _mapper.Map<List<VideoProgressDto>>(progresses);
        }

        public async Task<VideoProgressDto> GetByIdAsync(int id)
        {
            var progress = await _repository.GetByIdAsync(id);
            return _mapper.Map<VideoProgressDto>(progress);
        }

        public void Update(VideoProgressDto entity)
        {
            var progress = _mapper.Map<VideoProgress>(entity);
            _repository.Update(progress);
            _unitOfWorks.Commit();
        }
    }
}
