namespace Panda.SEOTracker.BusinessLogic
{
	internal static class AppMessages
	{
		/// <summary>
		///    Returns: <b>Unexpected error in repository.</b>
		/// </summary>
		internal static string Unexpected_error_in_repository { get; }
			= "Unexpected error in repository.";

		/// <summary>
		///    Returns: <b>Unable to create <paramref name="entityName"/>.</b>
		/// </summary>
		/// <param name="entityName"></param>
		internal static string UnableToCreate(string entityName)
			=> $"Unable to create {entityName}.";

		/// <summary>
		///    Returns: <b>Unable to delete <paramref name="entityName"/>.</b>
		/// </summary>
		/// <param name="entityName"></param>
		internal static string UnableToDelete(string entityName)
			=> $"Unable to delete {entityName}.";

		/// <summary>
		///    Returns: <b>Unable to get <paramref name="entityName"/>.</b>
		/// </summary>
		/// <param name="entityName"></param>
		internal static string UnableToGet(string entityName)
			=> $"Unable to get {entityName}.";

		/// <summary>
		///    Returns: <b>Unable to perform search on <paramref name="entityName"/>.</b>
		/// </summary>
		/// <param name="entityName"></param>
		internal static string UnableToPerformSearchOn(string entityName)
			=> $"Unable to perform search on {entityName}.";

		/// <summary>
		///    Returns: <b><paramref name="entityName"/> was not found.</b>
		/// </summary>
		/// <param name="entityName"></param>
		internal static string NotFound(string entityName)
			=> $"{entityName} was not found..";
	}
}