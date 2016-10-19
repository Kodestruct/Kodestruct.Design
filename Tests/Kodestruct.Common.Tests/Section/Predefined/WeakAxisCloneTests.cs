using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Common.Tests.Section.Predefined
{

    [TestFixture]
    public class SectionWeakAxisCloneTests : ToleranceTestBase
    {

        public SectionWeakAxisCloneTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;


        [Test]
        public void SectionRHSReturnsWeakAxisClone()
        {
            AiscShapeFactory factory = new AiscShapeFactory();
            ISectionTube section = factory.GetShape("HSS12X6X.375") as ISectionTube;
            ISection sectionClone = section.GetWeakAxisClone();
            double I_x = sectionClone.I_x;
            double refI_x = 72.9;
            //Manual gives 10.3 but actual area checked in Autocad is 10.42
            double actualTolerance = EvaluateActualTolerance(I_x, refI_x);
            Assert.LessOrEqual(actualTolerance, tolerance);
        }
    }
}
