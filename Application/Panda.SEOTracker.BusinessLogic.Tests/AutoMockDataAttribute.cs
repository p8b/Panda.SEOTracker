using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Panda.SEOTracker.BusinessLogic.Tests
{
	public class AutoMockDataAttribute : AutoDataAttribute
	{
		public AutoMockDataAttribute()
			: base(FixtureFactory)
		{
		}

		private static IFixture FixtureFactory()
		{
			var fixture = new Fixture();
			fixture.Customize(new AutoMoqCustomization()
			{
				ConfigureMembers = true,
				//GenerateDelegates = true
			});

			return fixture;
		}
	}
}