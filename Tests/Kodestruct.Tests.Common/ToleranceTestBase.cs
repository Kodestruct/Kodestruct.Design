using System;

namespace Kodestruct.Tests.Common
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
