using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFLicensing
{
    [Cmdlet(VerbsCommon.Get, "LFDSHost")]
    public class GetLFDSHost : DBCmdlet
    {
        [Parameter]
        public string ComputerName { get; set; }

        protected override void ProcessRecord()
        {
            var hosts = Database.GetAllHosts();
            if (ComputerName == null)
            {
                WriteObject(Database.GetAllHosts(), true);
            }
            else
            {
                var host = Database.GetHost(ComputerName);
                if (host != null)
                {
                    WriteObject(host);
                }
            }
        }
    }
}