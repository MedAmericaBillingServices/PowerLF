using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFServer
{
    public class ServerCmdlet : PSCmdlet
    {
        [Parameter]
        public LFAdminSession Session { get; set; }

        protected override void BeginProcessing()
        {
            Session = SessionState.PSVariable.GetValue("script:DefaultLFAdminSession", Session) as LFAdminSession;

            if (Session == null)
            {
                throw new PSArgumentException("No Laserfiche Admin Session", "Session");
            }
        }
    }
}
