using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Repository.Configurations
{
    public class VideoProgressConfiguration : IEntityTypeConfiguration<VideoProgress>
    {
        public void Configure(EntityTypeBuilder<VideoProgress> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PlaylistId)
                .IsRequired();

            builder.Property(x => x.VideoId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.IzlenmeYuzdesi)
                .IsRequired()
                .HasPrecision(5, 2); // 0.00 - 100.00

            builder.Property(x => x.IzlenenSaniye)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.ToplamSure)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(x => x.TamamlandiMi)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.IlkIzlemeTarihi)
                .IsRequired();

            builder.Property(x => x.SonIzlemeTarihi)
                .IsRequired();

            // User ile ilişki
            builder.HasOne(x => x.User)
                .WithMany(x => x.VideoProgresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Playlist ile ilişki
            builder.HasOne(x => x.Playlist)
                .WithMany()
                .HasForeignKey(x => x.PlaylistId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bir kullanıcının bir videoya ait sadece bir progress kaydı olabilir
            builder.HasIndex(x => new { x.UserId, x.VideoId })
                .IsUnique();

            builder.ToTable("VideoProgresses");
        }
    }
}
