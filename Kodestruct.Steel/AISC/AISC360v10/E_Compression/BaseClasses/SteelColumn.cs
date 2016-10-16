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

using Kodestruct.Steel.AISC.SteelEntities.Members;

namespace Kodestruct.Steel.AISC.AISC360v10
{
    public abstract class SteelColumn : SteelAxialMember
    {

            //    public SteelColumn(ISteelSection Section, double L_x, double L_y, double K_x, double K_y, ICalcLog CalcLog)
            //: base(Section, L_x, L_y,K_x,K_y, CalcLog)

        public SteelColumn(ISteelSection Section, double L_ex, double L_ey, ICalcLog CalcLog)
            : base(Section, L_ex, L_ey, CalcLog)
        {

        }


        protected double GetNominalAxialCapacity(double CriticalStress)
        {
            double A = Section.Shape.A;
            return CriticalStress*A;
        }

        protected double GetDesignAxialStrength(double CriticalStress)
        {
            double Pn= GetNominalAxialCapacity(CriticalStress);
            double phiP_n = Pn * 0.90; //per section E1 of specification
            return phiP_n;
        }
        public override double CalculateDesignStrength()
        {
            double phiP_n = 0.0;
            double Fcr = CalculateCriticalStress();
            phiP_n = GetDesignAxialStrength(Fcr);
            return phiP_n;
        }
    }
}
