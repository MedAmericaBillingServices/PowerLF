using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace PowerLFRepository
{
    [Cmdlet(VerbsCommon.New, "LFTrustee")]
    public class NewLFTrustee : RepositoryCmdlet
    {
        [Parameter(ParameterSetName = "ActiveDirectory")]
        public SwitchParameter ActiveDirectory { get; set; }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true,
            ParameterSetName = "ActiveDirectory")]
        public string Name { get; set; }

        [Parameter(Position = 1)]
        [ValidateSet("None", "Scan", "Export", "Print", "Search", "Delete", "Import", "AddTemplate", "AddVolume", "Move",
            "Process", "Edit", "AssignTemplate", "Migrate", "GetInformation", "ApplyWaterMarks", "AllTagFeatureRights",
            "All")]
        public string[] FeatureRights { get; set; } = {"None"};

        protected override void ProcessRecord()
        {
            WriteObject(Session.AddTrustee(GetSidForIdentity(Name), FeatureRights.ToList()));
        }
    }
}