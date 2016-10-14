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

using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using System;

namespace  Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElement: SteelDesignElement
    {
        
        public double GetVonMisesYieldCriterionInteractionRatio(double V_u, double phiV_n, double M_u, double phiM_n)
        {
            double DCR = Math.Pow(V_u / phiV_n, 2.0) + Math.Pow(M_u / phiM_n, 2.0);
            return DCR;
        }

   
    }
}
