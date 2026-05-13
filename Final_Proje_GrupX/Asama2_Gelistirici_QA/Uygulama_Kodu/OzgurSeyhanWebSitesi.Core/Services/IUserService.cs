using OzgurSeyhanWebSitesi.Core.Dtos.UserDtos;

namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IUserService : IGenericService<UserDto>
    {
        Task<UserDto> RegisterAsync(UserRegisterDto registerDto, string emailVerificationToken);
        Task<UserDto> LoginAsync(UserLoginDto loginDto);
        Task<UserDto> GoogleLoginAsync(string email, string googleId, string name);
        Task<UserDto> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task VerifyEmailAsync(string token);
    }
}
