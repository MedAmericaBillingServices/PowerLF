using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Laserfiche.Workflow;
using Laserfiche.Workflow.Objects;
using Laserfiche.Workflow.Objects.Options;

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

        internal void CreateProfile(string server, string repository, string username, string password, string profileName)
        {
            var profile = _workflowConnection.Settings.LaserficheProfiles.ConstructNewItem();
            profile.ProfileName = profileName;
            profile.ProfileServer = server;
            profile.ProfileRepository = repository;
            profile.ProfileUser = username;
            profile.SetProfilePassword(password);
            profile.Create();
        }
    }
}
