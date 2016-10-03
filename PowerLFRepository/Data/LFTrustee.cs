using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.RepositoryAccess;

namespace PowerLFRepository.Data
{
    public class LFTrustee : ILFPrincipal
    {
        private readonly TrusteeInfo _trusteeInfo;
        private readonly NTAccount _account;

        public string Name => _account.Value;

        public string FeatureRights => _trusteeInfo.FeatureRights.ToString();

        public DateTime LastLogOn => _trusteeInfo.LastLogOn;

        public IReadOnlyCollection<string> Groups { get; private set; }

        internal LFTrustee(TrusteeInfo trusteeInfo)
        {
            _trusteeInfo = trusteeInfo;
            _account = (NTAccount) _trusteeInfo.Sid.Translate(typeof(NTAccount));
            Groups = Trustee.EnumInheritedGroups(_trusteeInfo.Sid, _trusteeInfo.Session).Select(x => x.AccountName).ToList().AsReadOnly();
        }
    }
}
