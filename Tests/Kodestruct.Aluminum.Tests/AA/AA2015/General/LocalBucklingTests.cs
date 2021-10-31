using Kodestruct.Aluminum.AA.AA2015;
using Kodestruct.Aluminum.AA.AA2015.DesignRequirements;
using Kodestruct.Aluminum.AA.AA2015.DesignRequirements.LocalBuckling;
using Kodestruct.Aluminum.AA.AA2015.Flexure;
using Kodestruct.Aluminum.AA.Entities.Enums;
using Kodestruct.Aluminum.AA.Entities.Section;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kodestruct.Aluminum.Tests.AA.AA2015.General
{
 

    public class AluminumLocalBucklingTests : ToleranceTestBase
    {

            public AluminumLocalBucklingTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;


        [Fact]
        public void AluminumShapeReturnsBucklingConstant_C_p()
        {
            AluminumMaterial mat = new AluminumMaterial("6061","T6","0.062 or greater","std structural profile");
            MaterialBucklingConstantProvider bcf = new MaterialBucklingConstantProvider(BucklingType.FlexuralCompression, SubElementType.Flat, mat, Aluminum.AA.Entities.WeldCase.NotAffected);
            double C_p = bcf.C;
            double refValue = 66.9; // from Design Manual
            double actualTolerance = EvaluateActualTolerance(C_p, refValue);
            Assert.True(actualTolerance<=tolerance);
        }

        [Fact]
        public void AluminumElementReturnsLocalBucklingStressF_b()
        {
            //6063,T6,Up to 1,extrusion

            AluminumMaterial mat = new AluminumMaterial("6063", "T6", "Up to 1", "extrusion");
            //SectionBox sb = new SectionBox(null, 12, 4, 0.25, 0.25);
            //AluminumSection alSec =  new AluminumSection(mat,sb);
            //AluminumFlexuralMember m = new AluminumFlexuralMember(alSec);
            FlexuralLocalBucklingElement lbe = new FlexuralLocalBucklingElement(mat, 7, 0.25, LateralSupportType.OneEdge, 100, 80, Aluminum.AA.Entities.WeldCase.NotAffected);
            double F_b = lbe.GetCriticalStress();

            double refValue = 9.95; 
            double actualTolerance = EvaluateActualTolerance(F_b, refValue);
            Assert.True(actualTolerance<=tolerance);
        }

        [Fact]
        public void AluminumShapeReturnsLocalBucklingStressF_b()
        {
            //6063,T6,Up to 1,extrusion

            AluminumMaterial mat = new AluminumMaterial("6063", "T6", "Up to 1", "extrusion");
            SectionBox sb = new SectionBox(null, 12, 4, 0.25, 0.25);
            AluminumSection alSec = new AluminumSection(mat, sb);
            AluminumFlexuralMember m = new AluminumFlexuralMember(alSec);

            FlexuralLocalBucklingElement lbe = new FlexuralLocalBucklingElement(mat, 7, 0.25, LateralSupportType.OneEdge, 100, 80, Aluminum.AA.Entities.WeldCase.NotAffected);
            double F_b = m.GetLocalBucklingFlexuralCriticalStress(7, 0.25, LateralSupportType.OneEdge, Common.Section.Interfaces.FlexuralCompressionFiberPosition.Top,
                 Aluminum.AA.Entities.WeldCase.NotAffected).Value;

            double refValue = 9.95;
            double actualTolerance = EvaluateActualTolerance(F_b, refValue);
            Assert.True(actualTolerance<=tolerance);
        }

     }
}
