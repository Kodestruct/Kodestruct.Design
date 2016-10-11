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
using Kodestruct.Common.Section.SectionTypes;

namespace Kodestruct.Common.Tests.Section.ShapeTypes
{
    /// <summary>
    /// Compare calculated properties to W18X35 listed properties.
    /// </summary>
     [TestFixture]
    public class SectionIRolledTests : ToleranceTestBase
    {

         public SectionIRolledTests()
        {
            tolerance = 0.06; //6% can differ from fillet areas and rounding in the manual
        }

        double tolerance;


    [Test]
         public void SectionIRolledReturnsArea()
         {
             SectionIRolled shape = new SectionIRolled(null, 17.7, 6.0, 0.425, 0.3, 0.827);
             double A = shape.A;
             double refValue = 10.3;
             //Manual gives 10.3 but actual area checked in Autocad is 10.42
             double actualTolerance = EvaluateActualTolerance(A, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }

         [Test]
         public void SectionIRolledReturnsIx()
         {
             SectionIRolled shape = new SectionIRolled("", 17.7, 6.0, 0.425, 0.3, 0.827);
             double Ix = shape.I_x;
             //Manual gives 510 but actual area checked in Autocad is 540.0505
             double refValue = 510;
             double actualTolerance = EvaluateActualTolerance(Ix, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }

         [Test]
         public void SectionIRolledReturnsIy()
         {
             SectionIRolled shape = new SectionIRolled("", 17.7, 6.0, 0.425, 0.3, 0.827);
             double Iy = shape.I_y;
             double refValue = 15.3;
             double actualTolerance = EvaluateActualTolerance(Iy, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }

         [Test]
         public void SectionIRolledReturnsJ()
         {
             SectionI shape = new SectionI("", 12.2, 6.49, 0.38, 0.23);
             double J = shape.J;
             double refValue = 0.3;
             double actualTolerance = EvaluateActualTolerance(J, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }
         [Test]
         public void SectionIRolledReturnsC_w()
         {
             SectionI shape = new SectionI("", 12.2, 6.49, 0.38, 0.23);
             double C_w = shape.C_w;
             double refValue = 607.0;
             double actualTolerance = EvaluateActualTolerance(C_w, refValue);
             Assert.LessOrEqual(actualTolerance, tolerance);
         }
    }
    
}
