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
    public class OgretmenConfiguration : IEntityTypeConfiguration<Ogretmen>
    {
        public void Configure(EntityTypeBuilder<Ogretmen> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Soyad)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Yas)
                .IsRequired()
                .HasMaxLength(3);

            // Relationships
            builder.HasMany(x => x.Podcasts)
                .WithOne(x => x.Ogretmen)
                .HasForeignKey(x => x.OgretmenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.YoutubeVideolari)
                .WithOne(x => x.Ogretmen)
                .HasForeignKey(x => x.OgretmenId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.OzelDersler)
                .WithOne(x => x.Ogretmen)
                .HasForeignKey(x => x.OgretmenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table Name
            builder.ToTable("Ogretmenler");
        }
    }
}
