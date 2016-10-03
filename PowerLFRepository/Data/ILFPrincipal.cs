using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerLFRepository.Data
{
    public interface ILFPrincipal
    {
        string Name { get; }
        string FeatureRights { get; }
        DateTime LastLogOn { get; }
    }
}
