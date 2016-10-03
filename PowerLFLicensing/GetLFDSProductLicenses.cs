using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFLicensing
{
    [Cmdlet(VerbsCommon.Get, "LFDSProductLicenses")]
    public class GetLFDSProductLicenses : DBCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(Database.GetProductLicenses(), true);
        }
    }
}
