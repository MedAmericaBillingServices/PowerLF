using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFWorkflow
{
    public class WorkflowCmdlet : PSCmdlet
    {
        [Parameter]
        public LFWorkflowSession Session { get; set; }

        protected override void BeginProcessing()
        {
            Session = SessionState.PSVariable.GetValue("script:DefaultLFWorkflowSession", Session) as LFWorkflowSession;

            if (Session == null)
            {
                throw new PSArgumentException("No Laserfiche Workflow Session", "Session");
            }
        }
    }
}
