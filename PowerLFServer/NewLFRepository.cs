using System.Globalization;
using System.Management.Automation;

namespace PowerLFServer
{
    [Cmdlet(VerbsCommon.New, "LFRepository", DefaultParameterSetName = "IntegratedSecurity")]
    public class NewLFRepository : ServerCmdlet
    {

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string RepositoryName { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string DBMS { get; set; } = "SqlServer";

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string DatabaseServer { get; set; } = "localhost";

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string DatabaseName { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, ParameterSetName = "IntegratedSecurity")]
        public SwitchParameter IntegratedSecurity { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "SqlAuth")]
        public string SqlUsername { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "SqlAuth")]
        public string SqlPassword { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string RepositoryPath { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string DefaultVolumeName { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string DefaultVolumePath { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string SearchDirectory { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string SearchLanguage { get; set; } = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string SearchUrl { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public int SearchPort { get; set; } = 5053;

        [Parameter(ValueFromPipelineByPropertyName = true)]
        public bool CreateDatabase { get; set; } = true;

        protected override void ProcessRecord()
        {
            WriteObject(Session.CreateRepository(RepositoryName, DBMS, DatabaseServer, DatabaseName, SqlUsername,
                SqlPassword, RepositoryPath, DefaultVolumeName, DefaultVolumePath, SearchDirectory, SearchLanguage,
                SearchUrl, SearchPort, CreateDatabase));
        }

    }
}