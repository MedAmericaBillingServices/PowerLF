using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFRepository.Data
{
	public class LFVolumeInfo
	{
		public string FixedPath { get; set; }
		public int Id { get; internal set; }
		public bool IsCompressed { get; set; }
		public bool IsConnected { get; set; }
		public bool IsDataChecksummed { get; internal set; }
		public bool IsEDocFixed { get; internal set; }
		public bool IsImageFixed { get; internal set; }
		public bool IsLocked { get; internal set; }
		public bool IsMounted { get; set; }
		public bool IsOffline { get; internal set; }
		public int SourceVolumeId { get; internal set; }
		public bool IsPending { get; set; }
		public DateTime RolloverBaseDate { get; set; }
		public string Name { get; set; }
		public int RolloverPeriod { get; set; }
		public bool IsEncrypted { get; set; }
		public bool IsModified { get; set; }
		public bool IsNew { get; set; }
		public bool IsReadOnly { get; set; }
		public bool IsTextFixed { get; set; }
		public string LocalId { get; set; }
		public bool IsWriteOnce { get; set; }
		public long MaximumSize { get; set; }
		public string RemovablePath { get; set; }
		public DateTime RolloverDate { get; set; }
		public bool IsRollover { get; internal set; }
	}
}
