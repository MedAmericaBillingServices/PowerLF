using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFLicensing
{
    [Cmdlet(VerbsCommon.Get, "LFDSDatabase", DefaultParameterSetName = "All")]
    public class GetLFDSDatabase : ServerCmdlet
    {
        [Parameter(ParameterSetName = "ByRealm", Mandatory = true)]
        public string Realm { get; set; }

        [Parameter(ParameterSetName = "ByRealm")]
        public SwitchParameter NotDefault { get; set; }

        [Parameter]
        public PSCredential Credential { get; set; }

        protected override void ProcessRecord()
        {
            if (ParameterSetName == "ByRealm")
            {
                var db = Session.GetDatabase(Realm);
                if (Credential != null)
                {
                    var netCred = Credential.GetNetworkCredential();
                    db.Username = netCred.UserName;
                    db.Password = netCred.Password;
                }

                if (NotDefault == false)
                {
                    SessionState.PSVariable.Set("script:DefaultLFDSDatabase", db);
                }

                WriteObject(db);
            }
            else
            {
                WriteObject(Session.GetAllDatabases(), true);
            }
        }
    }
}
