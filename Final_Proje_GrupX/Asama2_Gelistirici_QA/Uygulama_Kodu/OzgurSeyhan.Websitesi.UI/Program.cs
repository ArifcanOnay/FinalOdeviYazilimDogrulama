using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;

namespace OzgurSeyhan.Websitesi.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Internal API Key - UI'dan API'ye yapılan isteklerde kimlik doğrulama için
            var internalApiKey = builder.Configuration["InternalApiKey"] ?? "";

            // HttpClient for API calls
            builder.Services.AddHttpClient("DefaultClient", client =>
                {
                    // Her istekte Internal API Key header'ı gönder
                    if (!string.IsNullOrEmpty(internalApiKey))
                    {
                        client.DefaultRequestHeaders.Add("X-Internal-Api-Key", internalApiKey);
                    }
                })
                .ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
                {
                    var handler = new HttpClientHandler();
                    
                    // SSL bypass sadece development ortamında - Production'da GÜVENLİK için bypass YOK!
                    var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
                    if (env.IsDevelopment())
                    {
                        handler.ServerCertificateCustomValidationCallback = 
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    }
                    
                    return handler;
                });
            
            builder.Services.AddHttpClient();

            // Cookie-based Authentication ve Google OAuth
            var googleClientId = builder.Configuration["GoogleAuth:ClientId"];
            var googleClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];

            var authBuilder = builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = ".OzgurSeyhan.Auth";
                options.LoginPath = "/Home/Login";
                options.LogoutPath = "/Home/Logout";
                options.AccessDeniedPath = "/Home/Index";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
            {
                authBuilder.AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleClientSecret;
                    options.CallbackPath = new PathString("/signin-google-callback");  // Explicit callback path
                    options.SaveTokens = true;
                    options.Scope.Clear();
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.CorrelationCookie.SameSite = SameSiteMode.None;
                    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                    
                    // Events ekle
                    options.Events.OnRemoteFailure = context =>
                    {
                        Console.WriteLine($"[GOOGLE FAIL] Exception: {context.Failure?.Message}");
                        Console.WriteLine($"[GOOGLE FAIL] Exception Type: {context.Failure?.GetType().Name}");
                        Console.WriteLine($"[GOOGLE FAIL] Request Path: {context.Request.Path}");
                        Console.WriteLine($"[GOOGLE FAIL] Query: {context.Request.QueryString}");
                        context.Response.Redirect("/Home/Login?error=" + Uri.EscapeDataString(context.Failure?.Message ?? "Unknown error"));
                        context.HandleResponse();
                        return System.Threading.Tasks.Task.CompletedTask;
                    };
                    
                    options.Events.OnTicketReceived = context =>
                    {
                        Console.WriteLine($"[GOOGLE SUCCESS] Ticket received for user: {context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value}");
                        return System.Threading.Tasks.Task.CompletedTask;
                    };
                    
                    options.Events.OnAccessDenied = context =>
                    {
                        Console.WriteLine($"[GOOGLE DENIED] User denied access");
                        context.Response.Redirect("/Home/Login?error=User+denied+access");
                        context.HandleResponse();
                        return System.Threading.Tasks.Task.CompletedTask;
                    };
                });

                builder.Services.PostConfigure<AuthenticationOptions>(options =>
                {
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                });
            }
            else
            {
                Console.WriteLine("[GOOGLE AUTH] ClientId/ClientSecret boş. Google login devre dışı bırakıldı.");
            }

            // Authorization Policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => 
                    policy.RequireClaim("IsAdmin", "true"));
            });

            // Session Support (sadece ekstra veri için)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(24);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = ".OzgurSeyhan.Session";
                options.Cookie.SameSite = SameSiteMode.None;  // Lax → None (OAuth callback sırasında gönderilsün)
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Güvenlik Headers Middleware ekle
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // IIS / Reverse Proxy arkasinda calismak icin
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
                    | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();
            
            // Güvenlik headers ekle
            app.Use(async (context, next) =>
            {
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
                context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";
                await next();
            });
            app.UseStaticFiles();
            app.UseRouting();
            
            // Authentication ve Authorization middleware sırası ÇOK ÖNEMLİ!
            app.UseAuthentication(); // ÖNCE bu
            app.UseAuthorization();  // SONRA bu
            
            // Session (sadece ekstra veri için)
            app.UseSession();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
