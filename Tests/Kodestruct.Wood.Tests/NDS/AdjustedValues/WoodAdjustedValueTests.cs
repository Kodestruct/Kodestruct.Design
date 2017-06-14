 
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Wood.NDS.NDS2015;




namespace Kodestruct.Wood.Tests.NDS.SawnLumber
{
    [TestFixture]
    public partial class WoodAdjustedValueTests : ToleranceTestBase
    {
        public WoodAdjustedValueTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;


    }
}
