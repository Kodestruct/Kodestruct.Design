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

namespace Kodestruct.Common.Mathematics
{
    public class Quad
    {
        public DataPoint2D LowerLeftPoint { get; set; }
        public DataPoint2D UpperLeftPoint { get; set; }
        public DataPoint2D LowerRightPoint { get; set; }
        public DataPoint2D UpperRightPoint { get; set; }

        public decimal GetInterpolatedValue(decimal XAbsolute, decimal YAbsolute, decimal InterpolationAdjustment=1.0m, 
            decimal AdjustmentProximityLength=1.0m)
            //if the value is interpolated an adjustment is introduced to account for the 
            //difference between USGS more precise calculation and this procedure
        {
            decimal x = XAbsolute;
            decimal y = YAbsolute;
            decimal x1 = LowerLeftPoint.X;
            decimal x2 = UpperRightPoint.X;
            decimal y1 = LowerLeftPoint.Y;
            decimal y2 = UpperRightPoint.Y;

           decimal R1 = ((x2-x)/(x2-x1))*LowerLeftPoint.Value + ((x-x1)/(x2-x1))*LowerRightPoint.Value;
           decimal R2 = ((x2-x)/(x2-x1))*UpperLeftPoint.Value + ((x-x1)/(x2-x1))*UpperRightPoint.Value;

           decimal P = ((y2 - y) / (y2 - y1)) * R1 + ((y - y1) / (y2 - y1)) * R2;

           DataPoint2D readingPoint = new DataPoint2D(XAbsolute, YAbsolute, 0);
            //it is assumed that the middle 2/3 need adjustment closer to the gridded points no adjustment is made
           decimal t = Math.Min(Math.Abs(LowerRightPoint.X - LowerLeftPoint.X), Math.Abs(LowerRightPoint.Y - UpperLeftPoint.Y)) / AdjustmentProximityLength;
           
            
            if (!LowerLeftPoint.IsNear(readingPoint, t) && !LowerRightPoint.IsNear(readingPoint, t)
               && !UpperLeftPoint.IsNear(readingPoint, t) && !UpperRightPoint.IsNear(readingPoint, t))
           {
               List<decimal> values = new List<decimal>()
            {
            LowerLeftPoint.Value,
            LowerRightPoint.Value,
            UpperLeftPoint.Value,
            UpperRightPoint.Value
            };
               decimal MaxVal = values.Max();
               decimal P_adjusted = P * InterpolationAdjustment;
               decimal P_final = Math.Min(MaxVal, P_adjusted);
               return P_final; 
           }
            else
            {
                return P;
            }
        }
    }
}
