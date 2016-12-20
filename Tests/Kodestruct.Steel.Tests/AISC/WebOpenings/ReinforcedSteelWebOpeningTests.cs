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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.Predefined;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Connections.WebOpenings;


namespace Kodestruct.Steel.Tests.AISC
{
    [TestFixture]
    public partial class ReinforcedWebOpeningTests : ToleranceTestBase
    {

        private void SetExampleValuesSteel()
        {

            d = 18.11;
            t_w = 0.39;
            //t_deck = 3.0;
            //t_fill = 2.5;
            t_e = 2.5;
            b_e = 120.0;
            h_0 = 11.0;
            a_o = 20.0;
            A_sn = 9.15;
            Q_n = 26.1;

            AiscShapeFactory f = new AiscShapeFactory();
            section = f.GetShape("W18X55") as ISectionI;
            F_y = 50.0;
            //f_cPrime = 4.0;
            //N_studs = 14.0;
            //N_o = 2.0;
            e = 0.0;
            t_r = 0.25;
            b_r = 0.65 / 0.25;
        }



        //4.6 EXAMPLE 2: COMPOSITE BEAM
        //WITH REINFORCED OPENING


        [Test]
        public void OpeningSteelReturnsShearStrength()
        {
            SetExampleValuesSteel();
            SteelIBeamWebOpening o = new SteelIBeamWebOpening(section, F_y,a_o,h_0,e,t_r,b_r,true);
            double phiV_n = o.GetShearStrength();
            double refValue =38.7;
            double actualTolerance = EvaluateActualTolerance(phiV_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
      
}
