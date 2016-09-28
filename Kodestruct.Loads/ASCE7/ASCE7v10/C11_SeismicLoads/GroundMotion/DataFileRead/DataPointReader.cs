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
using System.IO;
using System.Reflection;
using Kodestruct.Common.Data;
using Kodestruct.Common.Mathematics;
using System.Globalization;

namespace Kodestruct.Loads.ASCE.ASCE7_10.SeismicLoads
{
    public partial class SeismicGroundMotionMapDataReader : MapDataFileReader
    {

        private IMultipleValueDataPoint2D ReadDataPointFromResource(int position)
        {
            decimal Latitude, Longitude, SS, S1, TL;
            //string path = HostingEnvironment.MapPath("~/data");
            string path = "";
            string filepath = path + "/ASCE7_10_SeismicGroundMotionParameters.txt";
            var line = File.ReadLines(filepath).Skip(position - 1).Take(1).ToArray().First();

            string[] Vals = line.Split(',');
            if (Vals.Count() == 5)
            {
                try
                {
                    Latitude = decimal.Parse(Vals[0], CultureInfo.InvariantCulture);
                    Longitude = decimal.Parse(Vals[1], CultureInfo.InvariantCulture);
                    SS = decimal.Parse(Vals[2], CultureInfo.InvariantCulture);
                    S1 = decimal.Parse(Vals[3], CultureInfo.InvariantCulture);
                    TL = decimal.Parse(Vals[4], CultureInfo.InvariantCulture);
                    return new SeismicGroundMotionDataPoint(Latitude, Longitude, SS, S1, TL, position);
                }
                catch
                {

                    return new SeismicGroundMotionDataPoint();
                }
            }
            return new SeismicGroundMotionDataPoint();
        }

    }
}
