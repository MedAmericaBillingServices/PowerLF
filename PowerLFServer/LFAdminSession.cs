using System;
using System.Collections.Generic;
using System.Linq;
using Laserfiche.RepositoryAccess;
using Laserfiche.RepositoryAccess.Admin;

namespace PowerLFServer
{
    public class LFAdminSession
    {
        internal Server _server;

        public string Server => _server.Name;

        public LFAdminSession(string hostname, int port = 80)
        {
            _server = new Server(hostname, port) {ApplicationName = "PowerLF"};
        }

        public List<LFRepository> GetAllRepositories()
        {
            var repos = _server.GetRepositories();
            return repos.Select(x => new LFRepository(x, _server.GetRepository(x))).ToList();
        }

        public LFRepository GetRepository(string name)
        {
            var repos = _server.GetRepositories();
            var desiredRepo = repos.FirstOrDefault(x => x.Name == name);
            if (desiredRepo == null)
            {
                throw new ArgumentException("Unknown repository: " + name);
            }
            return new LFRepository(desiredRepo, _server.GetRepository(desiredRepo));
        }

        public LFRepository CreateRepository(string repositoryName, string dbms, string databaseServer,
            string databaseName, string sqlUsername, string sqlPassword, string repositoryPath, string defaultVolumeName,
            string defaultVolumePath, string searchDirectory, string searchLanguage, string searchUrl, int searchPort,
            bool shouldCreateDatabase)
        {
            SqlDbmsType dbmsType;

            if (!Enum.TryParse(dbms, out dbmsType))
            {
                throw new ArgumentException("Invalid DBMS type: " + dbms, nameof(dbms));
            }

            _server.CreateRepository(repositoryName, dbmsType, databaseServer, databaseName, sqlUsername, sqlPassword,
                repositoryPath, null, searchUrl, searchPort, searchDirectory, defaultVolumeName, defaultVolumePath,
                shouldCreateDatabase);

            var desiredRepo = _server.GetRepositories().FirstOrDefault(x => x.Name == repositoryName);
            return new LFRepository(desiredRepo, _server.GetRepository(desiredRepo));
        }

        public void RemoveRepository(LFRepository repository)
        {
            _server.DeleteRepository(repository.Name, DeleteRepositoryOptions.ForceUnmount);
        }

        public List<SessionInfo> GetConnections()
        {
            return _server.EnumSessions().ToList();
        }
    }
}