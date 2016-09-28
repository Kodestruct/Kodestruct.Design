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

namespace Kodestruct.Common.Section
{
    /// <summary>
    /// An implementation of compund shape when the profile type is not known.
    /// It is used for ISleceableShape implementation when a  slice of shape
    /// is generated and section properties for the slice are calculated.
    /// </summary>
    public class ArbitraryCompoundShape : CompoundShape
    {


        public ArbitraryCompoundShape(List<CompoundShapePart> rectanglesXAxis, List<CompoundShapePart> rectanglesYAxis)
        {

            if (rectanglesXAxis == null)
            {
                this.RectanglesXAxis = new List<CompoundShapePart>();
            }
            else
            {
                this.RectanglesXAxis = rectanglesXAxis;
            }

            if (rectanglesYAxis == null)
            {
                this.RectanglesYAxis = new List<CompoundShapePart>();
            }
            else
            {
                this.RectanglesYAxis = rectanglesYAxis;
            }

        }
        public override List<CompoundShapePart> GetCompoundRectangleXAxisList()
        {
            return RectanglesXAxis;
        }

        protected override void CalculateWarpingConstant()
        {
            _C_w = 0.0;
            torsionConstantCalculated = true;
        }

        public override List<CompoundShapePart> GetCompoundRectangleYAxisList()
        {
            return RectanglesYAxis;
        }
    }
}
