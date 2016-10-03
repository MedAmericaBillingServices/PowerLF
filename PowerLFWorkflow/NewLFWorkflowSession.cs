using Laserfiche.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFWorkflow
{
    [Cmdlet(VerbsCommon.New, "LFWorkflowSession")]
    public class NewLFWorkflowSession : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string ComputerName { get; set; }


        [Parameter]
        public SwitchParameter NotDefault { get; set; }

        protected override void ProcessRecord()
        {
            var session = new LFWorkflowSession(ComputerName);
            if (NotDefault == false)
            {
                SessionState.PSVariable.Set("script:DefaultLFWorkflowSession", session);
            }

            WriteObject(session);
        }
    }
}
