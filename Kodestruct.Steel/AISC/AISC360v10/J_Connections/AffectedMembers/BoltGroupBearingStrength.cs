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
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

using Kodestruct.Steel.AISC;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;

namespace Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElementWithHoles : SteelDesignElement
    {
        public double GetBoltGroupBearingStrength(double N_BoltRowParallel,double N_BoltRowPerpendicular,double phiR_nFirstRow, double phiR_nInnerRow)
        {
            double phiR_n = phiR_nFirstRow * N_BoltRowPerpendicular + phiR_nInnerRow * (N_BoltRowParallel - 1) * N_BoltRowPerpendicular;
            return phiR_n;
        }
    }
}
