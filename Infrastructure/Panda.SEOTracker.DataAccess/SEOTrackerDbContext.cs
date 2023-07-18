using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Panda.SEOTracker.Entities;

namespace Panda.SEOTracker.DataAccess
{
	public class SEOTrackerDbContext : DbContext
	{
		public DbSet<TrackedUrlEntity> TrackedUrls => Set<TrackedUrlEntity>();
		public DbSet<SearchTermHistoryEntity> SearchTermHistories => Set<SearchTermHistoryEntity>();
		public DbSet<SearchTermEntity> SearchTerms => Set<SearchTermEntity>();

		public SEOTrackerDbContext(DbContextOptions<SEOTrackerDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(
				Assembly.GetExecutingAssembly());
		}
	}
}