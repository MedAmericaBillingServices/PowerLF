using System.Management.Automation;

namespace PowerLFLicensing
{
    public class ServerCmdlet : PSCmdlet
    {
        [Parameter]
        public LFDSSession Session { get; set; }

        protected override void BeginProcessing()
        {
            Session = SessionState.PSVariable.GetValue("script:DefaultLFDSSession", Session) as LFDSSession;

            if (Session == null)
            {
                throw new PSArgumentException("No Laserfiche Directory Server Session", "Session");
            }
        }
    }
}