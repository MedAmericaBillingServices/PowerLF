using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Host;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.LicenseManager.LMO;

namespace PowerLFLicensing.Data
{
    public class LFLicenseDatabase
    {
        private readonly Database _database;
        internal AuthType AuthType;
        internal string Username;
        internal string Password;
        private bool _loggedin;

        public string Name => _database.Name;

        public string Realm => _database.RealmName;

        public string SQLServer => _database.SqlServer;

        public string SQLVersion => _database.DBMSType.ToString();

        public bool LicenseValid => _database.IsLicenseValid;

        internal LFLicenseDatabase(Database database) : this(database, AuthType.Windows)
        {
            
        }

        internal LFLicenseDatabase(Database database, AuthType authType, string username = null, string password = null)
        {
            _database = database;
            AuthType = authType;
            Username = username;
            Password = password;
            _loggedin = false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void EnsureLogin()
        {
            if (_loggedin) return;
            switch (AuthType)
            {
                    case AuthType.Windows:
                        if (Username == null)
                            _database.LoginWindows();
                        else
                            _database.LoginWindows(Username, Password);
                        break;
            }
            _loggedin = true;
        }

        internal List<LFProductLicense> GetProductLicenses()
        {
            EnsureLogin();

            var masterLicense = _database.GetMasterLicense();
            return masterLicense.ApplicationLimits.Select(x => new LFProductLicense(x.Value)).ToList();
        }

        internal List<LFRegisteredApplication> GetApplications()
        {
            EnsureLogin();
            var ml = _database.GetMasterLicense();

            var apps = _database.GetAllRegisteredApplications();
            var neededHosts = apps.Select(x => x.HostID).Distinct();
            var hosts = _database.GetAllHosts().Where(x => neededHosts.Contains(x.ID)).ToDictionary(x => x.ID);
            var licenses =
                ml.ApplicationLimits.Select(x => x.Key)
                    .Select(x => new LFProductLicense(ml.ApplicationLimits[x]))
                    .ToDictionary(x => x.GUID);

            return
                _database.GetAllRegisteredApplications()
                    .Select(x => new LFRegisteredApplication(x, hosts[x.HostID], licenses[x.LicenseBlockUuid]))
                    .ToList();
        }

        internal List<LFRegisteredApplication> GetApplicationsForHost(string hostname)
        {
            EnsureLogin();

            var host = GetHost(hostname);

            if (host == null)
            {
                throw new ArgumentException($"{hostname} not found in Laserfiche Directory Server", nameof(hostname));
            }

            return GetApplicationsForHost(host);
        }

        internal List<LFRegisteredApplication> GetApplicationsForHost(LFHost host)
        {
            EnsureLogin();

            var h = host.Host;

            var ml = _database.GetMasterLicense();

            var apps = _database.GetRegisteredApplicationsByHost(host.Host);
            var licenses =
                ml.ApplicationLimits.Select(x => x.Key)
                    .Select(x => new LFProductLicense(ml.ApplicationLimits[x]))
                    .ToDictionary(x => x.GUID);

            return
                apps.Select(x => new LFRegisteredApplication(x, host.Host, licenses[x.LicenseBlockUuid])).ToList();
        }

        internal List<LFRegisteredApplication> GetApplicationsForLicense(LFProductLicense license)
        {
            var apps = _database.GetRegisteredApplicationsByLimit(license.App);
            var neededHosts = apps.Select(x => x.HostID).Distinct();
            var hosts = _database.GetAllHosts().Where(x => neededHosts.Contains(x.ID)).ToDictionary(x => x.ID);

            return apps.Select(x => new LFRegisteredApplication(x, hosts[x.HostID], license)).ToList();
        }

        internal string GetLicenseFile(LFProductLicense license)
        {
            return "";
        }

        internal List<LFHost> GetAllHosts()
        {
            EnsureLogin();
            return _database.GetAllHosts().Select(x => new LFHost(x)).ToList();
        }

        internal LFHost GetHost(string hostname)
        {
            var hosts = GetAllHosts();

            var desiredHost = Dns.GetHostEntry(hostname);
            var hostsToSearch = hosts.Select(x => new {Hostname = Dns.GetHostEntry(x.Hostname), Host = x});

            var host =
                hostsToSearch.FirstOrDefault(
                    x =>
                        string.Equals(x.Hostname.HostName, desiredHost.HostName,
                            StringComparison.InvariantCultureIgnoreCase));

            return host?.Host;
        }

        internal LFHost CreateHost(string hostname)
        {
            EnsureLogin();
            var existingHost = GetHost(hostname);

            if (existingHost != null)
            {
                throw new ArgumentException($"{hostname} already registered in Laserfiche Directory Server",
                    nameof(hostname));
            }

            hostname = Dns.GetHostEntry(hostname).HostName.ToLowerInvariant();
            var fingerprint = HardwareFingerprint.GetFingerprint(hostname);

            var host = Host.Create(_database);
            host.Name = hostname;
            host.Fingerprint2 = fingerprint.HexData;
            host.Register();

            return new LFHost(host);
        }

        internal void RemoveHost(string hostname)
        {
            EnsureLogin();
            var host = GetHost(hostname);

            if (host == null)
            {
                throw new ArgumentException($"{hostname} not registered in Laserfiche Directory Server",
                    nameof(hostname));
            }

            host.Host.Unregister();
        }

        public LFRegisteredApplication RegisterApplication(string hostname, LFProductLicense productLicense, string version)
        {
            EnsureLogin();

            var host = GetHost(hostname);
            var appLimit = productLicense.App;
            var existingApp = GetApplicationsForHost(hostname).FirstOrDefault(x => x.ProductGUID == productLicense.GUID);

            if (existingApp != null)
            {
                throw new ArgumentException($"Application \"{productLicense.Name}\" is already registered on {hostname}", nameof(productLicense));
            }

            var app = Application.Create(_database, appLimit);
            app.HostID = host.ID;
            app.Version = version != null ? Version.Parse(version) : Version.Parse(appLimit.MaximumVersion);
            app.Register();

            return new LFRegisteredApplication(app, host.Host, productLicense);
        }

        internal LFRegisteredApplication RegisterApplication(LFHost host, LFProductLicense productLicense, string version)
        {
            EnsureLogin();

            var appLimit = productLicense.App;
            var existingApp = GetApplicationsForHost(host).FirstOrDefault(x => x.ProductGUID == productLicense.GUID);

            if (existingApp != null)
            {
                throw new ArgumentException($"Application \"{productLicense.Name}\" is already registered on {host.Hostname}", nameof(productLicense));
            }

            var app = Application.Create(_database, appLimit);
            app.HostID = host.ID;
            app.Version = version != null ? Version.Parse(version) : Version.Parse(appLimit.MaximumVersion);
            app.Register();

            return new LFRegisteredApplication(app, host.Host, productLicense);
        }

        internal void ExportLicenseFile(LFRegisteredApplication registeredApplication, string outFile)
        {
            var app = registeredApplication.App;
            var license = app.GetLicense(app.Version, LicenseType.Xml);
            File.WriteAllBytes(outFile, license);
        }
    }
}