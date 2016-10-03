using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.LicenseManager.LMO;

namespace PowerLFLicensing.Data
{
    public class LFRegisteredApplication
    {
        internal readonly Application App;
        private readonly Host _host;
        private readonly LFProductLicense _license;

        public string ComputerName => _host.Name;
        public string ProductName => _license.Name;
        public string[] AddOns => _license.AddOns;
        public ReadOnlyDictionary<string, int?> Features => _license.Features;
        public Guid ApplicationGUID => App.ApplicationUuid;
        public Guid ProductGUID => _license.GUID;


        internal LFRegisteredApplication(Application app, Host host, LFProductLicense license)
        {
            App = app;
            _host = host;
            _license = license;
        }
    }
}
