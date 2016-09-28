#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
   */
#endregion

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Connections;
using Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers;
using Kodestruct.Steel.AISC.SteelEntities.Materials;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.SpecialType
{
    [TestFixture]
    public class ExtendedSinglePlateTests: ToleranceTestBase
    {
        public ExtendedSinglePlateTests()
        {
            tolerance = 0.02; //2% can differ from rounding
        }

        double tolerance;

        public void ExtendedSinglePlateReturnsMaximumPlateThickness()
        {
            //double refValue = 5.79; // from AISC Steel Manual
            //double actualTolerance = EvaluateActualTolerance(C, refValue);

            //Assert.LessOrEqual(actualTolerance, tolerance);
        }

        //Thornton Fortney AISC EJ 2011. Example1.
        [Test]
        public void ExtendedPlateUnstiffenedReturnsBucklingStrength()
        {
            double d_pl = 24;
            double a = 9;
            double t_pl = 0.5;
            ExtendedSinglePlate sp = new ExtendedSinglePlate();
            double phiR_n = sp.GetShearStrengthWithoutStabilizerPlate(d_pl,t_pl,a,50.0);
            double refValue = 157;

            double actualTolerance = EvaluateActualTolerance(phiR_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }
        //Thornton Fortney AISC EJ 2011. Example6.
        [Test]
        public void ExtendedPlateReturnsTorsionalStrength()
        {
            double  R = 14; 
            double  l = 9; 
            double  a = 12;
            double  tp = 0.375; 
            double tw = 0.20;
            double  L = 233;
            double  bf = 3.97;
            ExtendedSinglePlate sp = new ExtendedSinglePlate();
            double phiM_n = sp.StabilizedExtendedSinglePlateTorsionalStrength(bf,50.0,L,R,tw,l,tp,50.0);
            double refValue = 18.6;

            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }

        //Tom Murray AISC webinar
        //Fundamentals of connection design
        //July 31, 2013 Part 4. Page 33.
        [Test]
        public void ExtendedPlateBucklingFlexuralStrength()
        {
            double phiR_n;
            double h_o = 9.0;
            double t_w = 0.5;
            SectionRectangular r = new SectionRectangular(t_w, h_o);
            SteelMaterial mat = new SteelMaterial(36);
            CalcLog log = new CalcLog();

            AffectedElementInFlexure flexuralElement = new AffectedElementInFlexure(r, mat, log);
            double lambda = flexuralElement.GetLambda(10);
            double refValue = 0.408;

            double actualTolerance = EvaluateActualTolerance(lambda, refValue);

            Assert.LessOrEqual(actualTolerance, tolerance);

        }


    }
}
