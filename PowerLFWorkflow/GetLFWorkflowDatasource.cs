using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFWorkflow
{
    [Cmdlet(VerbsCommon.Get, "LFWorkflowDatasource")]
    public class GetLFWorkflowDatasource : WorkflowCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(Session.GetAllDatasources(), true);
        }
    }
}
