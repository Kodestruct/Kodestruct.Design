 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kodestruct.Tests.Common;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Concrete.ACI;
using Kodestruct.Concrete.ACI318_14;
using Kodestruct.Concrete.ACI318_14.Materials;
using Kodestruct.Concrete.ACI.Entities;
using Kodestruct.Concrete.ACI318_14.Tests;
using Kodestruct.Concrete.ACI.ACI318_14.Durability.Cover;
using Xunit;

namespace Kodestruct.Concrete.ACI318_14.Cover
{
     
    public partial class AciConcreteCoverTests : ToleranceTestBase
    {
        private ICalcLog log;

        public ICalcLog Log
        {
            get { return log; }
            set { log = value; }
        }



        protected double tolerance;

        public AciConcreteCoverTests()
        {

            tolerance = 0.02; //2% can differ from rounding
        }


        [Fact]
        public void ExposedWallCastAginstEartReturnsValue()
        {

            RebarCoverFactory rcf = new RebarCoverFactory();
            double cc = rcf.GetRebarCover("NP-CIP-WALL-CastAgainstEarth", RebarDesignation.No5,false);
            double refValue = 3; 
            double actualTolerance = EvaluateActualTolerance(cc, refValue);

            Assert.True(actualTolerance <= tolerance);

        }


    }


}
