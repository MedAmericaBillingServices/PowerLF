using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFServer
{
    [Cmdlet(VerbsCommon.Get, "LFConnections")]
    public class GetLFConnections : ServerCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(Session.GetConnections(), true);
        }
    }
}
