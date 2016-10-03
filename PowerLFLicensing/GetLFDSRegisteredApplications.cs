using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using PowerLFLicensing.Data;

namespace PowerLFLicensing
{

    [Cmdlet(VerbsCommon.Get, "LFDSRegisteredApplications", DefaultParameterSetName = "All")]
    public class GetLFDSRegisteredApplications : DBCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "Host")]
        public string ComputerName { get; set; }

        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ParameterSetName = "License")]
        public LFProductLicense ProductLicense { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "All":
                    WriteObject(Database.GetApplications(), true);
                    break;
                case "Host":
                    WriteObject(Database.GetApplicationsForHost(ComputerName), true);
                    break;
                case "License":
                    WriteObject(Database.GetApplicationsForLicense(ProductLicense), true);
                    break;
            }
        }
    }
}
