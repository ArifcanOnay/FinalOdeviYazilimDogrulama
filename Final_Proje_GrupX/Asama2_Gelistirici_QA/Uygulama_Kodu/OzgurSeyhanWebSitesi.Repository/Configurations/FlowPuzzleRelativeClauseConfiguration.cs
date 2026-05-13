using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OzgurSeyhanWebSitesi.Core.Models;

namespace OzgurSeyhanWebSitesi.Repository.Configurations
{
    public class FlowPuzzleRelativeClauseConfiguration : IEntityTypeConfiguration<FlowPuzzleRelativeClause>
    {
        public void Configure(EntityTypeBuilder<FlowPuzzleRelativeClause> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.SourcePairEn)
                .IsRequired()
                .HasMaxLength(800);

            builder.Property(x => x.RelativeClauseEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.TurkishTranslation)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.SourcePairTr)
                .HasMaxLength(800);

            builder.Property(x => x.RelativeClauseTr)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.ToTable("FlowPuzzleRelativeClauses");
        }
    }
}
