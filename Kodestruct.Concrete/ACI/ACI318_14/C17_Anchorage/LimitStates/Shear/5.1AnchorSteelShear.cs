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
 
using Kodestruct.Concrete.ACI318_14.Anchorage.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Concrete.ACI318_14.Anchorage.LimitStates
{
    public class AnchorSteelShear: AnchorageSteelLimitState
    {


        public AnchorSteelShear(
            int n,
            double f_uta,
            double f_ya,
            double A_se_N,
            AnchorSteelElementFailureType SteelElementFailureType,
            AnchorInstallationType AnchorType, CastInAnchorageType CastInAnchorageType)
            : base(n, f_uta, f_ya, A_se_N, SteelElementFailureType, AnchorType)
        {

        }

        public override double GetNominalStrength()
        {
            //(17.4.1.2)
            List<double> stresses = new List<double>() { 125000, 1.9 * fya, futa };
            double N_sa = n * (A_se_N * stresses.Min() / 1000);
            return N_sa;
        }

        public double GetDesignStrength()
        {
            //17.3.3
            //cases (a) and (b)
            double phi = 1.0;
            if (SteelFailureType == AnchorSteelElementFailureType.Ductile)
            {
                phi = 0.65;
            }
            else
            {
                phi = 0.6;
            }

            return phi * GetNominalStrength();
        }


    }
}
