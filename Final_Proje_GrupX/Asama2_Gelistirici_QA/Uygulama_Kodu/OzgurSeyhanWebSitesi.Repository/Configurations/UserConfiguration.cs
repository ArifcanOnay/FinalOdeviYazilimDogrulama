using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Repository.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Soyad)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.KayitTarihi)
                .IsRequired();

            builder.Property(x => x.AktifMi)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.EmailDogrulandiMi)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.EmailDogrulamaToken)
                .HasMaxLength(200);

            builder.Property(x => x.EmailDogrulamaTokenExpiry);

            builder.Property(x => x.EmailDogrulamaTarihi);

            builder.HasIndex(x => x.EmailDogrulamaToken)
                .HasDatabaseName("IX_Users_EmailDogrulamaToken");

            builder.ToTable("Users");
        }
    }
}
