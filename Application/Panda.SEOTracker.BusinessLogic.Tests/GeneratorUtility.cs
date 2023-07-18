namespace Panda.SEOTracker.BusinessLogic.Tests
{
	public static class GeneratorUtility
	{
		public static string GetString(int length, char character = 'x')
			=> new(character, length);
	}
}