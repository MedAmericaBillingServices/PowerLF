using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.Workflow;
using Laserfiche.Workflow.Objects;

namespace PowerLFWorkflow
{
    public class LFWorkflowSession
    {
        private WorkflowConnection _workflowConnection;

        internal LFWorkflowSession(string hostname)
        {
            _workflowConnection = WorkflowConnection.CreateConnection(hostname, "PowerLF");
        }

        internal IEnumerable<DataSource> GetAllDatasources()
        {
            return _workflowConnection.Settings.DataSources.GetAllItems();
        }
    }
}
