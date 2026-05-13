namespace OzgurSeyhanWebSitesi.Core.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// E-posta doğrulama bağlantısı gönderir
        /// </summary>
        /// <param name="toEmail">Alıcı e-posta adresi</param>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="verificationUrl">Doğrulama URL'si</param>
        /// <returns></returns>
        Task SendVerificationEmailAsync(string toEmail, string userName, string verificationUrl);

        /// <summary>
        /// Şifre sıfırlama bağlantısı gönderir
        /// </summary>
        /// <param name="toEmail">Alıcı e-posta adresi</param>
        /// <param name="userName">Kullanıcı adı</param>
        /// <param name="resetUrl">Şifre sıfırlama URL'si</param>
        /// <returns></returns>
        Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetUrl);

        /// <summary>
        /// Genel amaçlı e-posta gönderir
        /// </summary>
        /// <param name="toEmail">Alıcı e-posta adresi</param>
        /// <param name="subject">E-posta konusu</param>
        /// <param name="htmlBody">HTML e-posta gövdesi</param>
        /// <returns></returns>
        Task SendEmailAsync(string toEmail, string subject, string htmlBody);

        /// <summary>
        /// Hoşgeldin e-postası gönderir
        /// </summary>
        /// <param name="toEmail">Alıcı e-posta adresi</param>
        /// <param name="userName">Kullanıcı adı</param>
        /// <returns></returns>
        Task SendWelcomeEmailAsync(string toEmail, string userName);
    }
}
