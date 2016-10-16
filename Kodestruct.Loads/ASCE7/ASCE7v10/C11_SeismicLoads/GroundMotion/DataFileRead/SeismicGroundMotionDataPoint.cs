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
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public class SeismicGroundMotionDataPoint : IMultipleValueDataPoint2D
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal SS { get; set; }
        public decimal S1 { get; set; }
        public decimal TL { get; set; }
        public int DataArrayIndex { get; set; }

        public SeismicGroundMotionDataPoint NextLongitudePoint { get; set; }
        public SeismicGroundMotionDataPoint NextLatitudePoint { get; set; }
        public SeismicGroundMotionDataPoint NextLatitudeAndLongitudePoint { get; set; }

        public SeismicGroundMotionDataPoint()
        {

        }
        //public SeismicGroundMotionDataPoint(decimal Latitude, decimal Longitude, decimal SS, decimal S1, decimal TL)
        //    :this( Latitude,  Longitude,  SS,  S1, TL, 1)
        //{
        //}

        public SeismicGroundMotionDataPoint(decimal Latitude, decimal Longitude, decimal SS, decimal S1, decimal TL, int Index)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.SS = SS;
            this.S1 = S1;
            this.TL = TL;
            this.DataArrayIndex = Index;
        }


        public decimal GetValue(object ValueParameter)
        {
            if (ValueParameter is GroundMotionParameterType)
            {
                GroundMotionParameterType valType = (GroundMotionParameterType)ValueParameter;
                switch (valType)
                {
                    case GroundMotionParameterType.SS:
                        return SS;
                    case GroundMotionParameterType.S1:
                        return S1;
                    case GroundMotionParameterType.TL:
                        return TL;
                    default:
                        return -1m;
                }
            }
            else
            {
                return -1m;
            }
        }


        public IMultipleValueDataPoint2D Clone()
        {
            SeismicGroundMotionDataPoint dp = new SeismicGroundMotionDataPoint()
            {
                    Latitude= this.Latitude,
                    Longitude=this.Longitude,
                    SS=this.SS,
                    S1=this.S1,
                    TL=this.TL,
                    DataArrayIndex=this.DataArrayIndex
            };
            return dp;
        }
    }
}
