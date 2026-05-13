using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Repository.Configurations
{
    public class FlowPuzzleSentenceConfiguration : IEntityTypeConfiguration<FlowPuzzleSentence>
    {
        public void Configure(EntityTypeBuilder<FlowPuzzleSentence> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Preposition)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.DeterminerSingular)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.DeterminerPlural)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.TurkishSingular)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.TurkishPlural)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Noun ile ilişki
            builder.HasOne(x => x.Noun)
                .WithMany()
                .HasForeignKey(x => x.NounId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("FlowPuzzleSentences");
        }
    }
}
