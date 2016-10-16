//Sample license text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI318_14.Tests
{
    public abstract class ToleranceTestBase
    {
        public double EvaluateActualTolerance(double C, double refValue)
        {
            double smallerVal = C < refValue ? C : refValue;
            double largerVal = C >= refValue ? C : refValue;
            double thisTolerance = largerVal / smallerVal - 1;

            return thisTolerance;
        }
    }
}
