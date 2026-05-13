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
    public class YoutubeVideoConfiguration : IEntityTypeConfiguration<YoutubeVideo>
    {
        public void Configure(EntityTypeBuilder<YoutubeVideo> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Id)
                .UseIdentityColumn();

            builder.Property(x => x.Baslik)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.VideoId)
                .IsRequired()
                .HasMaxLength(50);

            // Relationships
            builder.HasOne(x => x.Ogretmen)
                .WithMany(x => x.YoutubeVideolari)
                .HasForeignKey(x => x.OgretmenId)
                .OnDelete(DeleteBehavior.Cascade);

            // Table Name
            builder.ToTable("YoutubeVideos");
        }
    }
}
