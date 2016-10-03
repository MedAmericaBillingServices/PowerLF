using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Management.Automation;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFRepository
{
    public class RepositoryCmdlet : PSCmdlet
    {

        [Parameter]
        public LFSession Session { get; set; }

        protected RepositoryCmdlet()
        {
            
        }

        protected SecurityIdentifier GetSidForIdentity(string identity)
        {
            var ctx = new PrincipalContext(ContextType.Domain);
            var principal = Principal.FindByIdentity(ctx, identity);
            if (principal != null) return principal.Sid;

            try
            {
                var acct = new NTAccount(identity);
                return (SecurityIdentifier) acct.Translate(typeof(SecurityIdentifier));
            }
            catch
            {
                throw new ArgumentException($"Could not determine Security Identifier for {identity}", nameof(identity));
            }
        }

        protected override void BeginProcessing()
        {
            Session = SessionState.PSVariable.GetValue("script:DefaultLFSession", Session) as LFSession;
            if (Session == null)
            {
                throw new PSArgumentException("No Laserfiche Session", "Session");
            }
        }

    }
}
