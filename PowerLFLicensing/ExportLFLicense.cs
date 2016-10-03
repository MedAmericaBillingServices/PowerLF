using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using PowerLFLicensing.Data;

namespace PowerLFLicensing
{
    [Cmdlet(VerbsData.Export, "LFLicense")]
    public class ExportLFLicense : DBCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "ComputerName")]
        public string ComputerName { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "LFHost")]
        public LFHost LFHost { get; set; }

        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "ComputerName")]
        [Parameter(Position = 1, Mandatory = true, ParameterSetName = "LFHost")]
        public LFProductLicense ProductLicense { get; set; }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "RegisteredApplication", ValueFromPipeline = true)
        ]
        public LFRegisteredApplication RegisteredApplication { get; set; }

        [Parameter(Mandatory = true)]
        public string OutFile { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "ComputerName":
                    RegisteredApplication =
                        Database.GetApplicationsForHost(ComputerName)
                            .FirstOrDefault(x => x.ProductGUID == ProductLicense.GUID);
                    break;
                case "LFHost":
                    RegisteredApplication =
                        Database.GetApplicationsForHost(LFHost)
                            .FirstOrDefault(x => x.ProductGUID == ProductLicense.GUID);
                    break;
            }

            if (RegisteredApplication == null)
            {
                throw new PSArgumentException(
                    $"Could not find Registered Application for product {ProductLicense.Name} running on {ComputerName}",
                    nameof(ComputerName));
            }

            Database.ExportLicenseFile(RegisteredApplication, OutFile);
        }
    }
}