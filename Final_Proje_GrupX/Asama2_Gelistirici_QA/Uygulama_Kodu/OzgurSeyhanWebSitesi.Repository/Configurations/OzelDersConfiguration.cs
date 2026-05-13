using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzgurSeyhanWebSitesi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OzgurSeyhanWebSitesi.Repository.Configurations
{
    public class OzelDersConfiguration : IEntityTypeConfiguration<OzelDers>
    {
        public void Configure(EntityTypeBuilder<OzelDers> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.KurSeviyesi)
                .IsRequired()
                .HasMaxLength(400);

            builder.Property(x => x.Aciklama)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.HaftalikSaat)
                .IsRequired();

            builder.Property(x => x.MaksimumOgrenciSayisi)
                .IsRequired();

            builder.Property(x => x.Gunler)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.SaatAraligi)
                .IsRequired()
                .HasMaxLength(50);

            // Relationships
            builder.HasOne(x => x.Ogretmen)
                .WithMany(x => x.OzelDersler)
                .HasForeignKey(x => x.OgretmenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table Name
            builder.ToTable("OzelDersler");
        }
    }
}
