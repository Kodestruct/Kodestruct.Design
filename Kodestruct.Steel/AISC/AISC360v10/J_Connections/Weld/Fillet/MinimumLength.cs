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
using Kodestruct.Common.CalculationLogger.Interfaces;


namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public static partial class FilletWeldLimits
    {
        /// <summary>
        /// Minimum weld size per AISC Specification Section J2.2b
        /// </summary>
        /// <param name="size"></param>
        /// <param name="IsLongitudinalWeldsAloneInFlatBarTensionEnd">If longitudinal fillet welds are used alone in end connections of flat-bar tension members</param>
        /// <param name="perpendicularDistanceBetweenWelds"> Perpendicular distance between longitudianal welds </param>
        /// <returns></returns>
        public static double GetMinimumLengthOfWeld(double size, bool IsLongitudinalWeldsInTensionFlatBar, double perpendicularDistanceBetweenWelds)
        {
            double l_min = 1.0;
            double l1 = size * 4.0;

            if (IsLongitudinalWeldsInTensionFlatBar == true)
            {
                l_min = Math.Min(l1, perpendicularDistanceBetweenWelds);
            }
            else
            {
                l_min = l1;
            }
            return l_min;
        }
    }
}
