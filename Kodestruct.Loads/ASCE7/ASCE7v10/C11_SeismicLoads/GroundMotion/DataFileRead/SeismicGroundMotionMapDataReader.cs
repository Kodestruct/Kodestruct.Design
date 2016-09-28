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
using Kodestruct.Common.Data;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class SeismicGroundMotionMapDataReader : MapDataFileReader
    {
          //note ASCE 7 -10 for IBC 2012  
        public SeismicGroundMotionMapDataReader()
            : base(1, 611309, 1201, 0.05m)
        {

        }
       public  MapDataElement ReadData(decimal Latitude, decimal Longitude)
        {
            return base.ReadData(Latitude, Longitude, new ReadPointValueDelegate(ReadDataPointFromResource), 0.05m);
        }
    }
}
