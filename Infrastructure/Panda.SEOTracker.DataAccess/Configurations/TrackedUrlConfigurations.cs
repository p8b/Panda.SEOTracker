using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.DataAccess.Configurations;

internal class TrackedUrlConfigurations : IEntityTypeConfiguration<TrackedUrlEntity>
{
	public void Configure(EntityTypeBuilder<TrackedUrlEntity> builder)
	{
		builder.ToTable(TableNames.TrackedUrls, SchemaNames.SEO);
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasDefaultValueSql("NEWID()")
			.HasMaxLength(100);

		builder.Property(x => x.Url)
			.IsRequired()
			.HasMaxLength(256);

		builder.Property(x => x.TotalResultsToCheck)
			.IsRequired();

		builder.HasMany(x => x.SearchTerms)
			.WithOne()
			.HasForeignKey(x => x.TrackedUrlId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}