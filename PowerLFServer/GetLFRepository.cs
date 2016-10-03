using System.Management.Automation;

namespace PowerLFServer
{
	[Cmdlet(VerbsCommon.Get, "LFRepository")]
	public class GetLFRepository : ServerCmdlet
	{
		[Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public string Name { get; set; }

		protected override void ProcessRecord()
		{
			if (Name == null)
			{
				WriteObject(Session.GetAllRepositories(), true);
			}
			else
			{
				WriteObject(Session.GetRepository(Name));
			}
		}
	}
}
