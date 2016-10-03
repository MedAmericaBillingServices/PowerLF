using System.Management.Automation;

namespace PowerLFServer
{
    [Cmdlet(VerbsCommon.New, "LFAdminSession")]
    public class NewLFAdminSession : PSCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string ComputerName { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
        public int Port { get; set; }

        [Parameter]
        public SwitchParameter NotDefault { get; set; }

        protected override void ProcessRecord()
        {
            var session = Port == 0 ? new LFAdminSession(ComputerName) : new LFAdminSession(ComputerName, Port);

            if (NotDefault == false)
            {
                SessionState.PSVariable.Set("script:DefaultLFAdminSession", session);
            }

            WriteObject(session);
        }
    }
}