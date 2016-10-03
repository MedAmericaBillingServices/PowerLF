using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.LicenseManager.LMO;
using PowerLFLicensing.Data;
using LFDSServer = Laserfiche.LicenseManager.LMO.Server;

namespace PowerLFLicensing
{
    public class LFDSSession
    {
        private readonly LFDSServer _connection;

        public string Server { get; private set; }

        internal LFDSSession(string server, bool useSSL)
        {
            Server = server;
            _connection = LFDSServer.Connect(server, useSSL);
        }

        internal IEnumerable<LFLicenseDatabase> GetAllDatabases()
        {
            return _connection.GetAllDatabases().Select(x => new LFLicenseDatabase(x));
        }

        internal LFLicenseDatabase GetDatabase(string realm)
        {
            return new LFLicenseDatabase(_connection.GetDatabaseByRealmName(realm));
        }
    }
}