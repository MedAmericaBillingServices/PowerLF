using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.RepositoryAccess;

namespace PowerLFRepository
{
    [Cmdlet(VerbsCommon.Remove, "LFTrustee")]
    public class RemoveLFTrustee : RepositoryCmdlet
    {

        [Parameter(ParameterSetName = "ActiveDirectory")]
        public SwitchParameter ActiveDirectory { get; set; }
        
        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "ActiveDirectory")]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            var sid = GetSidForIdentity(Name);

            Session.RemoveTrustee(sid);

            WriteDebug($"Removed Trustee: {Name}");
        }
    }
}
