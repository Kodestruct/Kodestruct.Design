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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Compression;
using Kodestruct.Common.Mathematics;
using Kodestruct.Steel.AISC;

namespace Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElement
    {
        /// <summary>
        /// Effective length for gusset from AISC Design Guide 29 
        /// </summary>
        /// <param name="configuration">Type of gusset plate configuration for calculation of effective length</param>
        /// <param name="l_1">Gusset plate distance from beam to nearest row of bolts</param>
        /// <param name="l_2">Gusset plate distance from column to nearest row of bolts</param>
        /// <returns></returns>
        public double GetGussetPlateEffectiveCompressionLength(GussetPlateConfiguration configuration, double l_1, double l_2)
        {
            switch (configuration)
            {
                case GussetPlateConfiguration.CompactCorner:
                    return 0;
                    break;
                case GussetPlateConfiguration.NoncompactCorner:
                    return 1.0 * (l_1 + l_2) / 2.0;
                    break;
                case GussetPlateConfiguration.ExtendedCorner:
                    return 0.6 * l_1;
                    break;
                case GussetPlateConfiguration.SingleBrace:
                    return 0.7 * l_1;
                    break;
                case GussetPlateConfiguration.Chevron:
                    return 0.65 * l_1;
                    break;
                default:
                    return l_1 * 1.0;
                    break;
            }
        }
    }
}



