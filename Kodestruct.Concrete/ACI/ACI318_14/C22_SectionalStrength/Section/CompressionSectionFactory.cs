using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Mathematics;
using Kodestruct.Concrete.ACI318_14;

namespace Kodestruct.Concrete.ACI.ACI318_14
{
    public class CompressionSectionFactory
    {

        public ConcreteSectionCompression GetCompressionMember(ConcreteSectionFlexure flexuralSection,
        CompressionMemberType CompressionMemberType)
        {
            
            CalcLog log = new CalcLog();

            ConcreteSectionCompression compSection = new ConcreteSectionCompression(flexuralSection, CompressionMemberType, log);
            return compSection;
        }
      }

    
}
