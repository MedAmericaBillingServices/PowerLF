using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.Workflow.Objects.Forms;

namespace PowerLFWorkflow
{
    [Cmdlet(VerbsCommon.New, "LFWorkflowProfile")]
    public class NewLFWorkflowProfile : WorkflowCmdlet
    {
        [Parameter]
        public string LFServer { get; set; }
        [Parameter]
        public string Repository { get; set; }
        [Parameter]
        public string LFUsername { get; set; }
        [Parameter]
        public string LFPassword { get; set; }
        [Parameter]
        public string ProfileName { get; set; }

        protected override void ProcessRecord()
        {
            Session.CreateProfile(LFServer, Repository, LFUsername, LFPassword, ProfileName);
        }
    }
}
