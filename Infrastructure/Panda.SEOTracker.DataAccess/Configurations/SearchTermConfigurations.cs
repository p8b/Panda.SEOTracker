using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.DataAccess.Configurations;

internal class SearchTermConfigurations : IEntityTypeConfiguration<SearchTermEntity>
{
	public void Configure(EntityTypeBuilder<SearchTermEntity> builder)
	{
		builder.ToTable(TableNames.SearchTerms, SchemaNames.SEO);
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasDefaultValueSql("NEWID()")
			.HasMaxLength(100);

		builder.Property(x => x.Term)
			.IsRequired()
			.HasMaxLength(256);

		builder.HasMany(x => x.History)
			.WithOne()
			.HasForeignKey(x => x.SearchTermId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}