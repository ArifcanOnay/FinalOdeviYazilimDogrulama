using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzgurSeyhanWebSitesi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Repository.Configurations
{
    public class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.PlaylistId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Baslik)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Aciklama)
                .HasMaxLength(2000);

            builder.Property(x => x.KategoriBaslik)
                .HasMaxLength(200);

            // Ogretmen ile ilişki
            builder.HasOne(x => x.Ogretmen)
                .WithMany(x => x.Playlists)
                .HasForeignKey(x => x.OgretmenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Playlists");
        }
    }
}
