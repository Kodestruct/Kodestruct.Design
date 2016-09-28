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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamSolid : FlexuralMember, ISteelBeamFlexure
    {
        double GetM_y()
        {
            double S = GetS();

            double M_y = S * F_y;
            return M_y;
        }

        double GetS()
        {
            double S;
            if (MomentAxis == Common.Entities.MomentAxis.XAxis)
            {
                S = Math.Min(Section.Shape.S_xTop, Section.Shape.S_xBot);
            }
            else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
            {
                S = Math.Min(Section.Shape.S_yLeft, Section.Shape.S_yRight);
            }
            else
            {
                throw new FlexuralBendingAxisException();
            }
            return S;
        }

        double GetM_p()
        {
            double Z = 0;

            if (MomentAxis == Common.Entities.MomentAxis.XAxis)
            {
                Z = Section.Shape.Z_x;
            }
            else if (MomentAxis == Common.Entities.MomentAxis.YAxis)
            {
                Z = Section.Shape.Z_y;
            }
            else
            {
                throw new FlexuralBendingAxisException();
            }

            double M_p = Z * F_y;
            return M_p;
        }
    }
}
