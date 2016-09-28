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
using Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers.ConcentratedForces;

namespace Kodestruct.Steel.Tests.AISC.AISC360v10.Connections.AffectedMembers.ConcentratedForces
{

    [TestFixture]
    public class WebSideswayBucklingTests
    {
        [Test]
        public void WebSideswayBucklingReturnsValueW18()
        {
            double t_w = 0.425;
            double t_f = 0.68;
            double h_web = 16.84;
            double L_b_flange = 36.0;
            double M_y = 7300;
            double M_u = 8150;
            double b_f = 11.0;

            bool CompressionFlangeRestrained = true;

            double phiR_n = FlangeOrWebWithConcentratedForces.GetWebSideswayBucklingStrength(t_w, t_f, h_web, L_b_flange, b_f, CompressionFlangeRestrained, M_u, M_y);

        }
    }
}
