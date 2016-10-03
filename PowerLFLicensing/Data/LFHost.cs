using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.LicenseManager.LMO;

namespace PowerLFLicensing.Data
{
    public class LFHost
    {
        internal readonly Host Host;

        public string Hostname => Host.Name;

        public int ID => Host.ID;

        public string Fingerprint => Host.Fingerprint2;

        public string Comments => Host.Comments;

        internal LFHost(Host host)
        {
            Host = host;
        }
    }
}
