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
 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kodestruct.Steel.AISC.AISC360v10.Connections;
using Kodestruct.Tests.Common;
using Xunit;


namespace Kodestruct.Steel.Tests.AISC.AISC360v10.J_Connections.AffectedMembers.BeamCope
{
    //[TestFixture]
    public class BeamCopeTests: ToleranceTestBase
    {
        public BeamCopeTests()
        {
            tolerance = 0.05;
        }
        double tolerance;
     [Fact]
        public void BeamCopeReturnsValue()
        {
            //AISC Live Webinar:  FUNDAMENTALS OF CONNECTION DESIGN.  Tom Murray
            //August 21,  2014
            //Part 3 page 25 (Handout)
            double d = 13.8;
            double t_w = 0.270;
            double b_f = 6.73;
            double t_f = 0.385;
            double d_cope = 3.0;
            double c = 8;
            double F_y = 50;
            double F_u = 65;

            BeamCopeFactory factory = new BeamCopeFactory();
            IBeamCope copedBeam = factory.GetCope(Steel.AISC.BeamCopeCase.CopedTopFlange, d, b_f, t_f, t_w, d_cope, c, F_y, F_u);
            double phiM_n = copedBeam.GetFlexuralStrength();

            double refValue = 377; // from Lecture slides
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

     [Fact]
        public void BeamUncopedCopeReturnsValue()
        {
            //AISC Live Webinar:  FUNDAMENTALS OF CONNECTION DESIGN.  Tom Murray
            //August 21,  2014
            //Part 3 page 25 (Handout)
            double d = 13.8;
            double t_w = 0.270;
            double b_f = 6.73;
            double t_f = 0.385;
            double d_cope = 3.0;
            double c = 8;
            double F_y = 50;
            double F_u = 65;

            BeamCopeFactory factory = new BeamCopeFactory();
            IBeamCope copedBeam = factory.GetCope(Steel.AISC.BeamCopeCase.Uncoped, d, b_f, t_f, t_w, d_cope, c, F_y, F_u);
            double phiM_n = copedBeam.GetFlexuralStrength();

            double refValue = 2128; 
            double actualTolerance = EvaluateActualTolerance(phiM_n, refValue);

            Assert.True(actualTolerance<= tolerance);
        }

    }
}
