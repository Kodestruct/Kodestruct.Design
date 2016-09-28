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
using Kodestruct.Steel.AISC.AISC360v10.B_General;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public class IShapeFactory
    {
        public ISteelCompressionMember GetIshape(ISteelSection Section, bool IsRolled, double L_x, double L_y, double L_z, ICalcLog CalcLog)
        {

            ISteelCompressionMember column = null;
            IShapeCompactness compactnessTop = new ShapeCompactness.IShapeMember(Section, IsRolled, Common.Section.Interfaces.FlexuralCompressionFiberPosition.Top);
            IShapeCompactness compactnessBot = new ShapeCompactness.IShapeMember(Section, IsRolled, Common.Section.Interfaces.FlexuralCompressionFiberPosition.Bottom);
            ICalcLog Log = new CalcLog();


            CompactnessClassAxialCompression flangeCompactnessTop = compactnessTop.GetFlangeCompactnessCompression();
            CompactnessClassAxialCompression flangeCompactnessBot = compactnessTop.GetFlangeCompactnessCompression();
            CompactnessClassAxialCompression webCompactness = compactnessTop.GetWebCompactnessCompression();

            if (flangeCompactnessTop == CompactnessClassAxialCompression.NonSlender && webCompactness == CompactnessClassAxialCompression.NonSlender
                && flangeCompactnessBot == CompactnessClassAxialCompression.NonSlender)
            {
                column = new IShapeCompact(Section, IsRolled, L_x, L_y, L_z, Log);
                //if (IShape.b_fBot == IShape.b_fTop && IShape.t_fBot == IShape.t_fTop)
                //{
                //    return new IShapeCompact(SectionI, IsRolledShape, L_ex, L_ey, L_ez, log);
                //}
                //else
                //{
                //    throw new NotImplementedException();
                //}
            }
            else
            {
                column = new IShapeSlenderElements(Section, IsRolled, L_x, L_y, L_z, Log);
            }
            return column;
        }
    }
}
