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
using Kodestruct.Common.Mathematics;


namespace Kodestruct.Common.Data
{
    //this is the class used for looking up values in large map data files
    public class MapDataElement
    {
        private decimal latitude;

        public decimal Latitude
        {
            get { return latitude; }
        }

        private decimal longitude;

        public decimal Longitude
        {
            get { return longitude; }
        }
        

        public IMultipleValueDataPoint2D LowerLeftPoint { get; set; }
        public IMultipleValueDataPoint2D LowerRightPoint { get; set; }
        public IMultipleValueDataPoint2D UpperLeftPoint { get; set; }
        public IMultipleValueDataPoint2D UpperRightPoint { get; set; }

        public MapDataElement(decimal Latitude, decimal Longitude)
        {
            this.latitude = Latitude;
            this.longitude = Longitude;
        }
        public MapDataElement
            (
            decimal Latitude, decimal Longitude,
            IMultipleValueDataPoint2D LowerLeftPoint,
            IMultipleValueDataPoint2D LowerRightPoint,
            IMultipleValueDataPoint2D UpperLeftPoint,
            IMultipleValueDataPoint2D UpperRightPoint)
        {
            this.latitude = Latitude;
            this.longitude = Longitude;
            this.LowerLeftPoint = LowerLeftPoint;
            this.LowerRightPoint = LowerRightPoint;
            this.UpperLeftPoint = UpperLeftPoint;
            this.UpperRightPoint = UpperRightPoint;
        }
        public decimal GetValue(object ValueParameter, bool UseInterpolation = true, 
            decimal InterpolationAdjustment = 1.0m, decimal AdjustmentProximityLength = 1.0m)
        {
            decimal ValLowerLeftPoint = LowerLeftPoint.GetValue(ValueParameter);
            decimal ValLowerRightPoint = LowerRightPoint.GetValue(ValueParameter);
            decimal ValUpperLeftPoint = UpperLeftPoint.GetValue(ValueParameter);
            decimal ValUpperRightPoint = UpperRightPoint.GetValue(ValueParameter);

            decimal retVal;

            if (UseInterpolation==true)
            {
                Quad Quad = new Quad()
                {
                    LowerLeftPoint = new DataPoint2D(LowerLeftPoint.Longitude, LowerLeftPoint.Latitude, ValLowerLeftPoint),
                    LowerRightPoint = new DataPoint2D(LowerRightPoint.Longitude, LowerRightPoint.Latitude, ValLowerRightPoint),
                    UpperLeftPoint = new DataPoint2D(UpperLeftPoint.Longitude, UpperLeftPoint.Latitude, ValUpperLeftPoint),
                    UpperRightPoint = new DataPoint2D(UpperRightPoint.Longitude, UpperRightPoint.Latitude, ValUpperRightPoint)
                };

                retVal = Quad.GetInterpolatedValue(Longitude, Latitude, InterpolationAdjustment, AdjustmentProximityLength); 
            }
            else
            {
                List<decimal> ValList = new List<decimal>{ValLowerLeftPoint,ValLowerRightPoint,ValUpperLeftPoint,ValUpperRightPoint};
                retVal = ValList.Max();
            }
            return retVal;
        }
    }
}
