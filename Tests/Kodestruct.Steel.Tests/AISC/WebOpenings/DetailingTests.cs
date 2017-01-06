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
    public partial class WebOpeningTests : ToleranceTestBase
    {

        private void SetTestValues()
        {

            d = 15.9;
            t_w = 0.275;

            //t_deck = 3.0;
            //t_fill = 2.5;
            h_0 = 9.0;
            a_o = 20.0;


            AiscShapeFactory f = new AiscShapeFactory();
            section = f.GetShape("W16X36") as ISectionI;
            F_y = 50.0;
            //f_cPrime = 4.0;
            //N_studs = 14.0;
            //N_o = 2.0;
            e = 0.0;
            t_r = 0;
            b_r = 0;
            V_u = 0;
            M_u =0;
        }




        [Test]
        public void OpeningSteelReturnsMaxLength()
        {
            double d = 15.9;
            double t_w = 0.275;
            double t_f = 0.44;
           
            h_0 = 9.0;
            a_o = 20.0;
            double h_op = WebOpeningGeneral.GetMaximumOpeningHeight(a_o, d, e, t_f, t_w, F_y, true);

            double refValue =20;
            double actualTolerance = EvaluateActualTolerance(h_op, refValue);
            Assert.LessOrEqual(actualTolerance, tolerance);

        }
    }
      
}
