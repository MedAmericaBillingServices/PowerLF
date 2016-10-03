using Laserfiche.RepositoryAccess;

namespace PowerLFServer
{
	public class LFRepository
	{
		internal readonly RepositoryRegistration _registration;
		internal readonly Repository _repo;

		public string Name => _registration.Name;
		public string Server => _registration.ServerName;
		public string RepositoryPath => _repo.RepositoryPath;
		public string SearchServer => _repo.CatalogServer;
		public string SearchCatalog => _repo.CatalogName;
		public string SearchStatus => _repo.CatalogStatus.ToString();

		internal LFRepository(RepositoryRegistration registration, Repository repo)
		{
			_registration = registration;
			_repo = repo;
		}
	}
}
