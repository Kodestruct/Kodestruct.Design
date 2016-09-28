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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Composite
{
    public partial class CompositeBeamSection : AnalyticalElement
    {
        public double GetEffectiveSlabWidth(double L_beam, double L_centerLeft, double L_centerRight, double L_edgeLeft, double L_edgeRight)
        {
            //I3.1a. Effective Width
            //The effective width of the concrete slab shall be the sum of the effective widths for
            //each side of the beam centerline, each of which shall not exceed:
            //(1) one-eighth of the beam span, center-to-center of supports;
            //(2) one-half the distance to the centerline of the adjacent beam; or
            //(3) the distance to the edge of the slab.

            //if the edge distances are set to -1 then that distance does not govern
            L_centerLeft  =   L_centerLeft ==-1? double.PositiveInfinity :  L_centerLeft ;
            L_centerRight =   L_centerRight==-1? double.PositiveInfinity :  L_centerRight;
            L_edgeLeft    =   L_edgeLeft   ==-1? double.PositiveInfinity :  L_edgeLeft   ;
            L_edgeRight = L_edgeRight == -1 ? double.PositiveInfinity : L_edgeRight;

            List<double> LeftSideList = new List<double>() { L_beam / 8.0, L_edgeLeft, L_centerLeft / 2.0 };
            List<double>RightSideList = new List<double>() { L_beam / 8.0, L_edgeRight, L_centerRight / 2.0 };

            var b_eff = LeftSideList.Min() + RightSideList.Min();
            return b_eff;
        }
    }
}
