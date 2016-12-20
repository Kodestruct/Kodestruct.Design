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
    public class UnreinforcedWebOpeningTests : ToleranceTestBase
    {
        public UnreinforcedWebOpeningTests()
        {
            tolerance = 0.02; //2% can differ from rounding
            SetExampleValues();
        }

        private void SetExampleValues()
        {
            A_s = 13.0;
            d = 20.66;
            t_w = 0.35;
            t_s = 4.0;
            t_deck = 2.0;
            t_fill = 2.0;
            t_e = 2.0;
            b_e = 108.0;
            h_0 = 11.0;
            a_o = 22.0;
            s_b = 4.83;
            s_t = 4.83;
            DeltaA_s = 3.85;
            A_sn = 9.15;
            Q_n = 21.0;

            AiscShapeFactory f = new AiscShapeFactory();
            section = f.GetShape("W21X44") as ISectionI;
            F_y = 36.0;
            f_cPrime = 3.0;
            N_studs = 9;
            N_o = 1.0;
            e = 0.0;
            t_r = 0.0;
            b_r = 0.0;
        }

        double tolerance;

        //4.5 EXAMPLE 3: COMPOSITE BEAM
        //WITH UNREINFORCED OPENING
        double A_s;
        double d;
        double t_w;
        double t_s;
        double t_deck;
        double t_fill;
        double t_e;
        double b_e;
        double h_0;
        double a_o;
        double s_b;
        double s_t;
        double DeltaA_s;
        double A_sn;
        double Q_n;
        ISectionI section;
        double F_y;
        double f_cPrime;
        double N_studs;
        double N_o;
        double e;
        double t_r;
        double b_r;

        [Test]
        public void OpeningCompositeReturnsShearStrength()
        {
            SetExampleValues();
            CompositeIBeamWebOpening o = new CompositeIBeamWebOpening(section, b_e, t_fill, t_deck, F_y, f_cPrime, N_studs, Q_n,
                N_o, a_o, h_0, e, t_r, b_r, Steel.AISC.DeckAtBeamCondition.Perpendicular,4.5,12.0);
            double phiV_n = o.GetShearStrength();
            double refValue =30.94;
            double actualTolerance = EvaluateActualTolerance(phiV_n, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
      
}
