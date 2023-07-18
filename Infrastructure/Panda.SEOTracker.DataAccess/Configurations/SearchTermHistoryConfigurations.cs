using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.DataAccess.Configurations;

internal class SearchTermHistoryConfigurations : IEntityTypeConfiguration<SearchTermHistoryEntity>
{
	public void Configure(EntityTypeBuilder<SearchTermHistoryEntity> builder)
	{
		builder.ToTable(TableNames.SearchTermHistories, SchemaNames.SEO);
		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			.HasDefaultValueSql("NEWID()")
			.HasMaxLength(100);

		builder.Property(x => x.Date)
			.IsRequired();

		builder.Property(x => x.SearchEngineUsed)
			.IsRequired();

		builder.Property(x => x.Positions)
			.HasConversion(
			x => string.Join(',', x.Select(x => x)),
			x => Array.ConvertAll(x.Split(',', StringSplitOptions.RemoveEmptyEntries), x => int.Parse(x)).ToList() ?? new())
			.Metadata.SetValueComparer(
			new ValueComparer<ICollection<int>>(
				(c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
				c => c.ToHashSet()));
	}
}