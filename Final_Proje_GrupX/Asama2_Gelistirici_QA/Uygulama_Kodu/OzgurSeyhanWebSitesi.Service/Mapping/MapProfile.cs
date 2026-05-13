using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.OgretmenDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.OzelDersDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.PodcastDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.YoutubeVideoDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.PlaylistDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.UserDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.VideoProgressDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.HakkimdaDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleNounDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleSentenceDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleRelativeClauseDtos;
using OzgurSeyhanWebSitesi.Core.Dtos.FlowPuzzleLongPhraseDtos;
using OzgurSeyhanWebSitesi.Core.Dtos;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Bussinies.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // ==================== OGRETMEN MAPPINGS ====================
            CreateMap<Ogretmen, OgretmenDto>()
                .ReverseMap()
                .ForMember(dest => dest.Podcasts, opt => opt.Ignore())
                .ForMember(dest => dest.YoutubeVideolari, opt => opt.Ignore())
                .ForMember(dest => dest.OzelDersler, opt => opt.Ignore());
            
            CreateMap<CreateOgretmenDto, Ogretmen>();
            CreateMap<UpdateOgretmenDto, Ogretmen>();

            // ==================== OZEL DERS MAPPINGS ====================
            CreateMap<OzelDers, OzelDersDto>()
                .ReverseMap()
                .ForMember(dest => dest.Ogretmen, opt => opt.Ignore());
            
            CreateMap<CreateOzelDersDto, OzelDers>();
            CreateMap<UpdateOzelDersDto, OzelDers>();

            // ==================== PODCAST MAPPINGS ====================
            CreateMap<Podcast, PodcastDto>()
                .ReverseMap()
                .ForMember(dest => dest.Ogretmen, opt => opt.Ignore());
            
            CreateMap<CreatePodcastDto, Podcast>();
            CreateMap<UpdatePodcastDto, Podcast>();

            // ==================== YOUTUBE VIDEO MAPPINGS ====================
            CreateMap<YoutubeVideo, YoutubeVideoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Ogretmen, opt => opt.Ignore());
            
            CreateMap<CreateYoutubeVideoDto, YoutubeVideo>();
            CreateMap<UpdateYoutubeVideoDto, YoutubeVideo>();

            // ==================== PLAYLIST MAPPINGS ====================
            CreateMap<Playlist, PlaylistDto>()
                .ForMember(dest => dest.KapakResmi, opt => opt.Ignore()) // Servis katmanında set edilecek
                .ReverseMap()
                .ForMember(dest => dest.Ogretmen, opt => opt.Ignore());

            CreateMap<Playlist, PlaylistWithVideosDto>()
                .ForMember(dest => dest.KapakResmi, opt => opt.Ignore()) // Servis katmanında set edilecek
                .ForMember(dest => dest.Videos, opt => opt.Ignore()); // Videoları YouTube'dan dolduracağız

            // ==================== USER MAPPINGS ====================
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin)) // IsAdmin özellikle map edilsin
                .ReverseMap()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.EmailDogrulamaToken, opt => opt.Ignore())
                .ForMember(dest => dest.EmailDogrulamaTokenExpiry, opt => opt.Ignore())
                .ForMember(dest => dest.EmailDogrulamaTarihi, opt => opt.Ignore())
                .ForMember(dest => dest.VideoProgresses, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());
            
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.KayitTarihi, opt => opt.Ignore())
                .ForMember(dest => dest.SonGirisTarihi, opt => opt.Ignore())
                .ForMember(dest => dest.AktifMi, opt => opt.Ignore())
                .ForMember(dest => dest.EmailDogrulandiMi, opt => opt.Ignore())
                .ForMember(dest => dest.EmailDogrulamaToken, opt => opt.Ignore())
                .ForMember(dest => dest.EmailDogrulamaTokenExpiry, opt => opt.Ignore())
                .ForMember(dest => dest.EmailDogrulamaTarihi, opt => opt.Ignore())
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => false)) // Yeni kayıtta default false
                .ForMember(dest => dest.VideoProgresses, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());

            // ==================== VIDEO PROGRESS MAPPINGS ====================
            CreateMap<VideoProgress, VideoProgressDto>()
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Playlist, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());
            
            CreateMap<UpdateVideoProgressDto, VideoProgress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TamamlandiMi, opt => opt.Ignore())
                .ForMember(dest => dest.IlkIzlemeTarihi, opt => opt.Ignore())
                .ForMember(dest => dest.SonIzlemeTarihi, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Playlist, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateDate, opt => opt.Ignore());

            // ==================== REFERANS LINK MAPPINGS ====================
            CreateMap<ReferansLink, ReferansLinkDto>().ReverseMap();

            // ==================== HAKKIMDA MAPPINGS ====================
            CreateMap<Hakkimda, HakkimdaDto>().ReverseMap();
            CreateMap<UpdateHakkimdaDto, Hakkimda>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // ==================== FLOW PUZZLE NOUN MAPPINGS ====================
            CreateMap<FlowPuzzleNoun, FlowPuzzleNounDto>().ReverseMap();
            CreateMap<CreateFlowPuzzleNounDto, FlowPuzzleNoun>();
            CreateMap<UpdateFlowPuzzleNounDto, FlowPuzzleNoun>();

            // ==================== FLOW PUZZLE SENTENCE MAPPINGS ====================
            CreateMap<FlowPuzzleSentence, FlowPuzzleSentenceDto>()
                .ForMember(dest => dest.NounSingular, opt => opt.MapFrom(src => src.Noun.SingularForm))
                .ForMember(dest => dest.NounPlural, opt => opt.MapFrom(src => src.Noun.PluralForm));
            CreateMap<CreateFlowPuzzleSentenceDto, FlowPuzzleSentence>();
            CreateMap<UpdateFlowPuzzleSentenceDto, FlowPuzzleSentence>();

            // ==================== FLOW PUZZLE RELATIVE CLAUSE MAPPINGS ====================
            CreateMap<FlowPuzzleRelativeClause, FlowPuzzleRelativeClauseDto>().ReverseMap();
            CreateMap<CreateFlowPuzzleRelativeClauseDto, FlowPuzzleRelativeClause>();
            CreateMap<UpdateFlowPuzzleRelativeClauseDto, FlowPuzzleRelativeClause>();

            // ==================== FLOW PUZZLE LONG PHRASE MAPPINGS ====================
            CreateMap<FlowPuzzleLongPhrase, FlowPuzzleLongPhraseDto>().ReverseMap();
            CreateMap<CreateFlowPuzzleLongPhraseDto, FlowPuzzleLongPhrase>();
            CreateMap<UpdateFlowPuzzleLongPhraseDto, FlowPuzzleLongPhrase>();
        }
    }
}
