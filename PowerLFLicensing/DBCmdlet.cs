using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using PowerLFLicensing.Data;

namespace PowerLFLicensing
{
    public class DBCmdlet : PSCmdlet
    {
        [Parameter]
        public LFLicenseDatabase Database { get; set; }

        protected override void BeginProcessing()
        {
            Database = SessionState.PSVariable.GetValue("script:DefaultLFDSDatabase", Database) as LFLicenseDatabase;

            if (Database == null)
            {
                throw new PSArgumentException("No Laserfiche Directory Server Database", "Database");
            }
        }
    }
}