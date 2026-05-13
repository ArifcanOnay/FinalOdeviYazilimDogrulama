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
    public class PodcastConfiguration : IEntityTypeConfiguration<Podcast>
    {
        public void Configure(EntityTypeBuilder<Podcast> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.Baslik)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PodcastUrl)
                .IsRequired()
                .HasMaxLength(500);

            // Relationships
            builder.HasOne(x => x.Ogretmen)
                .WithMany(x => x.Podcasts)
                .HasForeignKey(x => x.OgretmenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table Name
            builder.ToTable("Podcasts");
        }
    }
}
