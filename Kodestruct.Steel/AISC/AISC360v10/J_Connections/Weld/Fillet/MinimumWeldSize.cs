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
using Kodestruct.Common.CalculationLogger.Interfaces;


namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public static partial class FilletWeldLimits
    {
        /// <summary>
        /// Gets Minimum Size of Fillet Welds per Table J2.4
        /// </summary>
        /// <param name="MaterialThickness"> Thickness of connected material</param>
        /// <returns></returns>
        public static double GetMinimumWeldSize(double MaterialThickness)
        {
            double tmin = 0;
            if (MaterialThickness <= 1 / 4)
            {
                tmin = 1 / 8.0;
            }
            else
            {
                if (MaterialThickness > 3 / 4)
                {
                    tmin = 5 / 16.0;
                }
                else if (tmin <= 1 / 2)
                {
                    tmin = 3 / 16.0;
                }
                else
                {
                    tmin = 1 / 4.0;
                }
            }
            return tmin;
        }
    }
}
