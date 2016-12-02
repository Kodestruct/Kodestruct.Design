using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Common.Section.Interfaces
{
    public interface IFirstMomentOfAreaCalculatable : ISliceableSection
    {
        double GetFirstMomentOfAreaX(double TopOffset);
    }
}
