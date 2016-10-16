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
using Kodestruct.Steel.AISC.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Bolted
{
    public abstract partial class BoltSlipCritical : Bolt, IBoltSlipCritical
    {

        /// <summary>
        /// Gets increment  C2 in accordance with Table J3.5
        /// </summary>
        /// <returns></returns>
        public double GetEdgeDistanceIncrementC2()
        {
            double s = 0.0;
            switch (HoleType)
            {
                case Kodestruct.Steel.AISC.SteelEntities.Bolts.BoltHoleType.STD:
                    s = 0.0;
                    break;
                case Kodestruct.Steel.AISC.SteelEntities.Bolts.BoltHoleType.SSL_Perpendicular:
                    if (Diameter <= 1)
                    {
                        s = 1 / 8.0;
                    }
                    else
                    {
                        s = 3 / 16.0;
                    }
                    break;
                case Kodestruct.Steel.AISC.SteelEntities.Bolts.BoltHoleType.SSL_Parallel:
                    s = 0.0;
                    break;
                case Kodestruct.Steel.AISC.SteelEntities.Bolts.BoltHoleType.OVS:
                    if (Diameter>=1)
                    {
                        s = 1 / 8.0;
                    }
                    else
                    {
                        s = 1 / 16.0;
                    }
                    break;
                case Kodestruct.Steel.AISC.SteelEntities.Bolts.BoltHoleType.LSL_Perpendicular:
                    s = 3.0 / 4.0 * Diameter;
                    break;
                case Kodestruct.Steel.AISC.SteelEntities.Bolts.BoltHoleType.LSL_Parallel:
                    s = 0.0;
                    break;
                default:
                    break;
            }
            return s;
        }

        /// <summary>
        /// Gets minimum edge distance
        /// </summary>
        /// <returns></returns>
        public double GetMinimumEdgeDistance()
        {
            double s = 0.0;

            // The distance from the center of an oversized
            // or slotted hole to an edge of a connected part shall be not less than that required for
            // a standard hole to an edge of a connected part plus the applicable increment, C2, from
            // Table J3.5
            s = GetStandardHoleMinimumEdgeDistance() + GetEdgeDistanceIncrementC2();
            return s;
        }
    }
}
