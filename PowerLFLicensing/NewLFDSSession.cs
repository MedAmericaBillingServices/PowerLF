using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFLicensing
{
    [Cmdlet(VerbsCommon.New, "LFDSSession")]
    public class NewLFDSSession : PSCmdlet
    {

        [Parameter(Mandatory = true, Position = 0)]
        public string ComputerName { get; set; }

        [Parameter]
        public SwitchParameter UseSSL { get; set; }

        [Parameter]
        public SwitchParameter NotDefault { get; set; }

        protected override void BeginProcessing()
        {
            var session = new LFDSSession(ComputerName, UseSSL);

            if (NotDefault == false)
            {
                SessionState.PSVariable.Set("script:DefaultLFDSSession", session);
            }

            WriteObject(session);
        }
    }
}
