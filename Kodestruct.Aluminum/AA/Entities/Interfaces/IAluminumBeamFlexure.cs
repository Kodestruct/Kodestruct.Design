
using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.Entities.Interfaces
{


    public interface IAluminumBeamFlexure
    {
        AluminumLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation);
        AluminumLimitStateValue GetFlexuralRuptureStrength();
    }
}
