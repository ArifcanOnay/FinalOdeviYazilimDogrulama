using AutoMapper;
using OzgurSeyhanWebSitesi.Core.Dtos.UserDtos;
using OzgurSeyhanWebSitesi.Core.Models;
using OzgurSeyhanWebSitesi.Core.Repositories;
using OzgurSeyhanWebSitesi.Core.Services;
using OzgurSeyhanWebSitesi.Core.UnitOfWorks;
using BCrypt.Net;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class UserService : IUserService
    {
        private static readonly TimeSpan EmailVerificationTokenLifetime = TimeSpan.FromHours(24);

        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<User> _repository;
        private readonly IUnitOfWorks _unitOfWorks;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UserService(
            IGenericRepository<User> repository, 
            IUnitOfWorks unitOfWorks, 
            IMapper mapper, 
            IUserRepository userRepository,
            IEmailService emailService)
        {
            _repository = repository;
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto registerDto, string emailVerificationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null && existingUser.EmailDogrulandiMi)
            {
                throw new Exception("Bu email adresi zaten kayıtlı!");
            }

            if (existingUser != null && !existingUser.EmailDogrulandiMi)
            {
                existingUser.Ad = registerDto.Ad;
                existingUser.Soyad = registerDto.Soyad;
                existingUser.PasswordHash = HashPassword(registerDto.Password);
                existingUser.EmailDogrulamaToken = emailVerificationToken;
                existingUser.EmailDogrulamaTokenExpiry = DateTime.UtcNow.Add(EmailVerificationTokenLifetime);
                existingUser.EmailDogrulamaTarihi = null;
                existingUser.UpdateDate = DateTime.UtcNow;

                _repository.Update(existingUser);
                await _unitOfWorks.CommitAsync();

                return _mapper.Map<UserDto>(existingUser);
            }

            var user = _mapper.Map<User>(registerDto);
            user.PasswordHash = HashPassword(registerDto.Password);
            user.KayitTarihi = DateTime.UtcNow;
            user.AktifMi = true;
            user.EmailDogrulandiMi = false;
            user.EmailDogrulamaToken = emailVerificationToken;
            user.EmailDogrulamaTokenExpiry = DateTime.UtcNow.Add(EmailVerificationTokenLifetime);
            user.EmailDogrulamaTarihi = null;
            user.CreateDate = DateTime.UtcNow;
            user.UpdateDate = DateTime.UtcNow;

            await _userRepository.AddAsync(user);
            await _unitOfWorks.CommitAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> LoginAsync(UserLoginDto loginDto)
        {
            Console.WriteLine($"[UserService.LoginAsync] Email: {loginDto.Email}");
            
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            
            if (user == null)
            {
                Console.WriteLine($"[UserService.LoginAsync] ❌ Kullanıcı bulunamadı: {loginDto.Email}");
                throw new Exception("Email veya şifre hatalı!");
            }

            Console.WriteLine($"[UserService.LoginAsync] ✅ Kullanıcı bulundu: Id={user.Id}, Email={user.Email}, AktifMi={user.AktifMi}");
            Console.WriteLine($"[UserService.LoginAsync] Password hash var mı: {!string.IsNullOrEmpty(user.PasswordHash)}");
            Console.WriteLine($"[UserService.LoginAsync] Password hash uzunluğu: {user.PasswordHash?.Length ?? 0}");

            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                Console.WriteLine($"[UserService.LoginAsync] ❌ Şifre doğrulama başarısız!");
                throw new Exception("Email veya şifre hatalı!");
            }

            if (!user.EmailDogrulandiMi)
            {
                throw new Exception("E-posta adresiniz henüz doğrulanmamış. Lütfen mail kutunuzu kontrol edip doğrulama linkine tıklayın.");
            }

            Console.WriteLine($"[UserService.LoginAsync] ✅ Şifre doğrulandı!");

            if (user.AktifMi == false)
            {
                Console.WriteLine($"[UserService.LoginAsync] ❌ Hesap aktif değil!");
                throw new Exception("Hesabınız aktif değil!");
            }

            // Son giriş tarihini güncelle
            user.SonGirisTarihi = DateTime.UtcNow;
            await _unitOfWorks.CommitAsync();

            Console.WriteLine($"[UserService.LoginAsync] ✅ Login başarılı! UserId={user.Id}");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GoogleLoginAsync(string email, string googleId, string name)
        {
            Console.WriteLine($"[UserService.GoogleLoginAsync] Email: {email}, GoogleId: {googleId}");

            // Kullanıcıyı email'e göre bul
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                // Yeni kullanıcı oluştur
                Console.WriteLine($"[UserService.GoogleLoginAsync] Yeni kullanıcı oluşturuluyor...");
                
                // Ad ve Soyadı ayır
                var nameParts = name?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                var firstName = nameParts.Length > 0 ? nameParts[0] : "Google";
                var lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "User";

                user = new User
                {
                    Email = email,
                    Ad = firstName,
                    Soyad = lastName,
                    PasswordHash = HashPassword(googleId), // Google ID'yi şifre olarak sakla (kullanılmayacak)
                    KayitTarihi = DateTime.UtcNow,
                    AktifMi = true,
                    EmailDogrulandiMi = true, // Google tarafından doğrulanmış olarak işaretle
                    EmailDogrulamaTarihi = DateTime.UtcNow,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    SonGirisTarihi = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);
                await _unitOfWorks.CommitAsync();
                Console.WriteLine($"[UserService.GoogleLoginAsync] ✅ Yeni kullanıcı oluşturuldu! UserId={user.Id}");
            }
            else
            {
                // Mevcut kullanıcı ile giriş yap
                Console.WriteLine($"[UserService.GoogleLoginAsync] ✅ Mevcut kullanıcı bulundu! UserId={user.Id}");

                if (!user.AktifMi)
                {
                    throw new Exception("Hesabınız aktif değil!");
                }

                // Son giriş tarihini güncelle
                user.SonGirisTarihi = DateTime.UtcNow;
                user.UpdateDate = DateTime.UtcNow;
                await _unitOfWorks.CommitAsync();
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task VerifyEmailAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new Exception("Doğrulama bağlantısı geçersiz.");
            }

            var user = await _userRepository.GetByEmailVerificationTokenAsync(token);
            if (user == null)
            {
                throw new Exception("Doğrulama bağlantısı geçersiz veya kullanılmış.");
            }

            if (user.EmailDogrulandiMi)
            {
                return;
            }

            if (!user.EmailDogrulamaTokenExpiry.HasValue || user.EmailDogrulamaTokenExpiry.Value < DateTime.UtcNow)
            {
                throw new Exception("Doğrulama bağlantısının süresi dolmuş. Lütfen yeniden kayıt olmayı deneyin.");
            }

            user.EmailDogrulandiMi = true;
            user.EmailDogrulamaTarihi = DateTime.UtcNow;
            user.EmailDogrulamaToken = null;
            user.EmailDogrulamaTokenExpiry = null;
            user.UpdateDate = DateTime.UtcNow;

            _repository.Update(user);
            await _unitOfWorks.CommitAsync();
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepository.EmailExistsAsync(email);
        }

        private string HashPassword(string password)
        {
            // BCrypt ile güvenli şifre hashleme (workFactor: 12)
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        private bool VerifyPassword(string password, string hash)
        {
            // BCrypt ile şifre doğrulama
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }

        // IGenericService implementations
        public async Task AddAsync(UserDto entity)
        {
            var user = _mapper.Map<User>(entity);
            await _repository.AddAsync(user);
            await _unitOfWorks.CommitAsync();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _unitOfWorks.Commit();
        }

        public List<UserDto> GetAll()
        {
            var users = _repository.GetAll();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public void Update(UserDto entity)
        {
            var user = _mapper.Map<User>(entity);
            _repository.Update(user);
            _unitOfWorks.Commit();
        }
    }
}
