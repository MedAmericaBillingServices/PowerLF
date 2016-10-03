using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Management.Automation;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PowerLFRepository.Data;

namespace PowerLFRepository
{
    [Cmdlet(VerbsCommon.Get, "LFTrustee", DefaultParameterSetName = ADParameterSet)]
    public class GetLFTrustee : RepositoryCmdlet
    {
        private const string ADParameterSet = "Active Directory";

        [Parameter(ParameterSetName = ADParameterSet)]
        public SwitchParameter ActiveDirectory { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = ADParameterSet)]
        public string Name { get; set; }



        protected override void ProcessRecord()
        {
            if (ParameterSetName == ADParameterSet && Name == null)
            {
                WriteObject(Session.GetAllTrustees(), true);
            }
            else if (ParameterSetName == ADParameterSet && Name != null)
            {
                SecurityIdentifier sid = null;
                try
                {
                    sid = GetSidForIdentity(Name);
                }
                catch (ArgumentException e)
                {
                    throw new PSArgumentException($"Could not find {Name} in Active Directory", nameof(Name));
                }
                WriteObject(Session.GetTrustee(sid), true);
            }
        }
    }
}