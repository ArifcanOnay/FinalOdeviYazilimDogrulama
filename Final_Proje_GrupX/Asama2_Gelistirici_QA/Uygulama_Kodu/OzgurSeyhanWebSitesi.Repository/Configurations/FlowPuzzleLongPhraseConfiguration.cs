using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Repository.Configurations
{
    public class FlowPuzzleLongPhraseConfiguration : IEntityTypeConfiguration<FlowPuzzleLongPhrase>
    {
        public void Configure(EntityTypeBuilder<FlowPuzzleLongPhrase> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.HeadNounEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.OfPhraseEn)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.DetailPhraseEn)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.ContextPhraseEn)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.FullPhraseEn)
                .IsRequired()
                .HasMaxLength(900);

            builder.Property(x => x.TurkishTranslation)
                .IsRequired()
                .HasMaxLength(900);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.ToTable("FlowPuzzleLongPhrases");
        }
    }
}
