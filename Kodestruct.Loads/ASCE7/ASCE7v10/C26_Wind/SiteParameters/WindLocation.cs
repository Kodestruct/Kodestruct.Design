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
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindLocation : AnalyticalElement
    {

        public WindLocation(ICalcLog CalcLog) : this(CalcLog,0,0)
        {

        }
        public WindLocation(ICalcLog CalcLog, double Latitude, double Longitude, string County=null)
            : base(CalcLog)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
            this.County = County;
        }
        bool IsDirty;

        private double longitude;

        public double Longitude
        {
            get { return longitude; }
            set
            {
                longitude = value;
                IsDirty = true;
            }
        }

        private double latitude;

        public double Latitude
        {
            get { return latitude; }
            set
            {
                latitude = value;
                IsDirty = true;
            }
        }

        public string County { get; set; }
    }
}
