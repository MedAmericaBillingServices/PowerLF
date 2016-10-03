using System.DirectoryServices.AccountManagement;
using System.Management.Automation;

namespace PowerLFRepository
{
    [Cmdlet(VerbsCommon.Get, "LFUser")]
    public class GetLFUser : PSCmdlet
    {
        [Parameter]
        public LFSession Session { get; set; }

        [Parameter()]
        public string Username { get; set; }

        protected override void BeginProcessing()
        {
            Session = Session ?? (SessionState.PSVariable.Get("script:DefaultLFSession").Value as LFSession);
            if (Session == null)
            {
                throw new PSArgumentException("No Laserfiche Session", "Session");
            }
        }

        protected override void ProcessRecord()
        {
            if (Username != null)
            {
                var principal = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain), Username);
                WriteObject(principal != null ? Session.GetUser(principal) : Session.GetUser(Username));
            }
            else
            {
                Session.GetAllUsers().ForEach(WriteObject);
            }
        }
    }
}