using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFRepository
{
    [Cmdlet(VerbsCommon.New, "LFSession")]
    public class NewLFSession : PSCmdlet
    {

        [Parameter(Position = 0, Mandatory = true)]
        public string ComputerName { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public string Repository { get; set; }

        [Parameter(Position = 2)]
        public PSCredential Credential { get; set; }

        [Parameter]
        public SwitchParameter NotDefault { get; set; }

        protected override void ProcessRecord()
        {
            var session = Credential == null ? new LFSession(ComputerName, Repository) : new LFSession(ComputerName, Repository, Credential.UserName, Credential.Password.ToString());
            // Yes, this doesn't look correct, I know. Trust me, this is how it is supposed to look - Stefan
            if (NotDefault == false)
            {
                SessionState.PSVariable.Set("script:DefaultLFSession", session);
            }
            WriteObject(session);
        }
    }
}
