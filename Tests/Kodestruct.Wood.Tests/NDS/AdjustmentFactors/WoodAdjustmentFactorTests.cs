 
using Kodestruct.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Wood.NDS.NDS2015;




namespace Kodestruct.Wood.Tests.NDS.SawnLumber
{
     
    public partial class WoodAdjustmentFactorTests : ToleranceTestBase
    {
        public WoodAdjustmentFactorTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;

 
    }
}
