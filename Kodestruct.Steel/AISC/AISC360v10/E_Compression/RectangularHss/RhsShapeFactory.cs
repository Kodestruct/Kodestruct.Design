#region Copyright
   /*Copyright (C) 2015 Konstantin Udilovich

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
using Kodestruct.Steel.AISC.AISC360v10.B_General;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public class RhsShapeFactory
    {
        public ISteelCompressionMember GetRhsShape(ISteelSection Section, double L_ex, double L_ey, double L_ez, ICalcLog CalcLog)
        {

            ISteelCompressionMember column = null;
            IShapeCompactness compactnessX = new ShapeCompactness.HollowMember(Section, Common.Section.Interfaces.FlexuralCompressionFiberPosition.Top, Common.Entities.MomentAxis.XAxis);
            IShapeCompactness compactnessY = new ShapeCompactness.HollowMember(Section, Common.Section.Interfaces.FlexuralCompressionFiberPosition.Top, Common.Entities.MomentAxis.YAxis);
            ICalcLog Log = new CalcLog();

            CompactnessClassAxialCompression webCompactnessX = compactnessX.GetWebCompactnessCompression();
            CompactnessClassAxialCompression webCompactnessY = compactnessY.GetWebCompactnessCompression();

            if (webCompactnessX ==  CompactnessClassAxialCompression.NonSlender && webCompactnessY == CompactnessClassAxialCompression.NonSlender )
            {
                return new RhsNonSlender(Section, L_ex, L_ey, L_ez, CalcLog);
            }
            else
            {
                return new RhsSlender(Section, L_ex, L_ey, L_ez, CalcLog);
            }
            return column;
        }
    }
}
