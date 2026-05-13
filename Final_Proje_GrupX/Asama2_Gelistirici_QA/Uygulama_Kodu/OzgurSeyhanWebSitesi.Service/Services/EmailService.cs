using System.Net.Mail;
using System.Net;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OzgurSeyhanWebSitesi.Core.Services;

namespace OzgurSeyhanWebSitesi.Bussinies.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendVerificationEmailAsync(string toEmail, string userName, string verificationUrl)
        {
            var subject = "E-posta Adresinizi Doğrulayın";
            var htmlBody = GetVerificationEmailTemplate(userName, verificationUrl);
            await SendEmailAsync(toEmail, subject, htmlBody);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string userName, string resetUrl)
        {
            var subject = "Şifrenizi Sıfırlayın";
            var htmlBody = GetPasswordResetEmailTemplate(userName, resetUrl);
            await SendEmailAsync(toEmail, subject, htmlBody);
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string userName)
        {
            var subject = "Hoşgeldiniz! 5 Dakikada İngilizce";
            var htmlBody = GetWelcomeEmailTemplate(userName);
            await SendEmailAsync(toEmail, subject, htmlBody);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            try
            {
                // SMTP ayarlarını oku
                var smtpConfig = GetSmtpConfiguration();

                // Validation
                if (string.IsNullOrWhiteSpace(smtpConfig.Host)
                    || string.IsNullOrWhiteSpace(smtpConfig.User)
                    || string.IsNullOrWhiteSpace(smtpConfig.Password)
                    || string.IsNullOrWhiteSpace(smtpConfig.FromEmail))
                {
                    _logger.LogError("SMTP ayarları eksik veya yanlış yapılandırılmış.");
                    throw new InvalidOperationException("Mail servisi yapılandırılmamış. SMTP ayarlarını tamamlayın.");
                }

                using var message = new MailMessage
                {
                    From = new MailAddress(smtpConfig.FromEmail, smtpConfig.FromName),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = htmlBody
                };

                message.To.Add(toEmail);

                using var smtpClient = new SmtpClient(smtpConfig.Host, smtpConfig.Port)
                {
                    Credentials = new NetworkCredential(smtpConfig.User, smtpConfig.Password),
                    EnableSsl = smtpConfig.EnableSsl
                };

                _logger.LogInformation("E-posta gönderiliyor: {ToEmail}, Konu: {Subject}", toEmail, subject);
                await smtpClient.SendMailAsync(message);
                _logger.LogInformation("E-posta başarıyla gönderildi: {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "E-posta gönderilirken hata oluştu. Alıcı: {ToEmail}, Konu: {Subject}", toEmail, subject);
                throw;
            }
        }

        /// <summary>
        /// SMTP konfigürasyonunu ortam değişkenlerinden veya appsettings'ten okur
        /// </summary>
        private SmtpConfiguration GetSmtpConfiguration()
        {
            var host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? _configuration["EmailSettings:SmtpHost"];
            var portRaw = Environment.GetEnvironmentVariable("SMTP_PORT") ?? _configuration["EmailSettings:SmtpPort"];
            var user = Environment.GetEnvironmentVariable("SMTP_USER") ?? _configuration["EmailSettings:SmtpUser"];
            var password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? _configuration["EmailSettings:SmtpPassword"];
            var fromEmail = Environment.GetEnvironmentVariable("SMTP_FROM_EMAIL") ?? _configuration["EmailSettings:FromEmail"];
            var fromName = Environment.GetEnvironmentVariable("SMTP_FROM_NAME") ?? _configuration["EmailSettings:FromName"] ?? "5 Dakikada İngilizce";
            var enableSslRaw = Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL") ?? _configuration["EmailSettings:EnableSsl"];

            var port = int.TryParse(portRaw, out var parsedPort) ? parsedPort : 587;
            var enableSsl = !bool.TryParse(enableSslRaw, out var parsedSsl) || parsedSsl;

            return new SmtpConfiguration
            {
                Host = host,
                Port = port,
                User = user,
                Password = password,
                FromEmail = fromEmail,
                FromName = fromName,
                EnableSsl = enableSsl
            };
        }

        /// <summary>
        /// E-posta doğrulama şablonu
        /// </summary>
        private string GetVerificationEmailTemplate(string userName, string verificationUrl)
        {
            return $@"
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9f9f9; }}
        .header {{ background: #2e7d32; color: white; padding: 20px; border-radius: 8px 8px 0 0; text-align: center; }}
        .content {{ background: white; padding: 30px; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; background: #2e7d32; color: white; padding: 12px 20px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; border-top: 1px solid #ddd; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>E-posta Doğrulaması</h2>
        </div>
        <div class='content'>
            <h3>Merhaba {HttpUtility.HtmlEncode(userName)},</h3>
            <p>Hesabınızı tamamlamak için e-posta adresinizi doğrulamanız gerekiyor.</p>
            <p style='text-align: center; margin: 30px 0;'>
                <a href='{HttpUtility.HtmlEncode(verificationUrl)}' class='button'>E-postamı Doğrula</a>
            </p>
            <p><strong>Bu bağlantı 24 saat boyunca geçerlidir.</strong></p>
            <p>Eğer bu kaydı siz yapmadıysanız, bu e-postayı dikkate almayabilirsiniz.</p>
            <hr style='border: none; border-top: 1px solid #ddd; margin: 20px 0;'>
            <p style='font-size: 12px; color: #666;'>
                <strong>Doğrulama Linki:</strong><br>
                <a href='{HttpUtility.HtmlEncode(verificationUrl)}' style='word-break: break-all;'>{HttpUtility.HtmlEncode(verificationUrl)}</a>
            </p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 5 Dakikada İngilizce. Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Şifre sıfırlama şablonu
        /// </summary>
        private string GetPasswordResetEmailTemplate(string userName, string resetUrl)
        {
            return $@"
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9f9f9; }}
        .header {{ background: #d32f2f; color: white; padding: 20px; border-radius: 8px 8px 0 0; text-align: center; }}
        .content {{ background: white; padding: 30px; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; background: #d32f2f; color: white; padding: 12px 20px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; border-top: 1px solid #ddd; margin-top: 20px; }}
        .warning {{ background: #fff3cd; border-left: 4px solid #ffc107; padding: 10px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>Şifre Sıfırlama Talebi</h2>
        </div>
        <div class='content'>
            <h3>Merhaba {HttpUtility.HtmlEncode(userName)},</h3>
            <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayınız.</p>
            <p style='text-align: center; margin: 30px 0;'>
                <a href='{HttpUtility.HtmlEncode(resetUrl)}' class='button'>Şifremi Sıfırla</a>
            </p>
            <div class='warning'>
                <strong>⚠️ Önemli:</strong> Bu bağlantı 1 saat boyunca geçerlidir. Eğer bu talebini siz yapmadıysanız, lütfen 
                <a href='mailto:support@example.com'>destek ekibine</a> başvurunuz.
            </div>
            <p><strong>Sıfırlama Linki:</strong><br>
                <a href='{HttpUtility.HtmlEncode(resetUrl)}' style='word-break: break-all;'>{HttpUtility.HtmlEncode(resetUrl)}</a>
            </p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 5 Dakikada İngilizce. Tüm hakları saklıdır.</p>
        </div>
    </div>
</body>
</html>";
        }

        /// <summary>
        /// Hoşgeldin e-postası şablonu
        /// </summary>
        private string GetWelcomeEmailTemplate(string userName)
        {
            return $@"
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background: #f9f9f9; }}
        .header {{ background: linear-gradient(135deg, #2e7d32 0%, #1b5e20 100%); color: white; padding: 40px 20px; border-radius: 8px 8px 0 0; text-align: center; }}
        .content {{ background: white; padding: 30px; border-radius: 0 0 8px 8px; }}
        .button {{ display: inline-block; background: #2e7d32; color: white; padding: 12px 20px; text-decoration: none; border-radius: 5px; margin: 15px 0; }}
        .footer {{ text-align: center; padding: 20px; font-size: 12px; color: #666; border-top: 1px solid #ddd; margin-top: 20px; }}
        .features {{ list-style: none; padding: 0; }}
        .features li {{ padding: 8px 0; padding-left: 25px; position: relative; }}
        .features li:before {{ content: '✓'; position: absolute; left: 0; color: #2e7d32; font-weight: bold; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Hoşgeldiniz! 🎉</h1>
            <p>5 Dakikada İngilizce Ailesine Katıldığınız İçin Teşekkürler</p>
        </div>
        <div class='content'>
            <h2>Merhaba {HttpUtility.HtmlEncode(userName)},</h2>
            <p>Hesabınız başarıyla oluşturuldu! Artık aşağıdaki avantajlardan yararlanmaya başlayabilirsiniz:</p>
            <ul class='features'>
                <li>Binlerce İngilizce ders ve alıştırma</li>
                <li>Kişisel öğrenme ilerlemenizi takip edin</li>
                <li>Interaktif sınavlarla seviyenizi belirleyin</li>
                <li>Video derslerle pratik yapın</li>
                <li>Özel öğretmen ve danışmanlık hizmetleri</li>
            </ul>
            <p style='text-align: center; margin: 30px 0;'>
                <a href='https://example.com/dashboard' class='button'>Şimdi Başla</a>
            </p>
            <p>Herhangi bir sorunuz olursa, bizim destek ekibimize ulaşmaktan çekinmeyin.</p>
            <p>Mutlu öğrenmeler!</p>
        </div>
        <div class='footer'>
            <p>&copy; 2026 5 Dakikada İngilizce. Tüm hakları saklıdır.</p>
            <p><a href='https://example.com/contact'>İletişim</a> | <a href='https://example.com/privacy'>Gizlilik Politikası</a></p>
        </div>
    </div>
</body>
</html>";
        }
    }

    /// <summary>
    /// SMTP konfigürasyonu için yardımcı sınıf
    /// </summary>
    internal class SmtpConfiguration
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public string? FromEmail { get; set; }
        public string? FromName { get; set; }
        public bool EnableSsl { get; set; }
    }
}
