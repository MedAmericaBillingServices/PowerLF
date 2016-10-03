using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFLicensing
{

    [Cmdlet(VerbsCommon.New, "LFDSHost")]
    public class NewLFDSHost : DBCmdlet
    {

        [Parameter(Position = 0, Mandatory = true)]
        public string ComputerName { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Database.CreateHost(ComputerName));
        }
    }

}
