using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using PowerLFLicensing.Data;

namespace PowerLFLicensing
{
    [Cmdlet(VerbsCommon.New, "LFDSRegisteredApplication")]
    public class NewLFDSRegisteredApplication : DBCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ComputerName")]
        public string ComputerName { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Host")]
        public LFHost LFHost { get; set; }

        [Parameter(Position = 1, Mandatory = true)]
        public LFProductLicense ProductLicense { get; set; }

        [Parameter(Position = 2)]
        public string Version { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ComputerName":
                    WriteObject(Database.RegisterApplication(ComputerName, ProductLicense, Version));
                    break;
                case "Host":
                    WriteObject(Database.RegisterApplication(LFHost, ProductLicense, Version));
                    break;
            }
        }
    }
}
