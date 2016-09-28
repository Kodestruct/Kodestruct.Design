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
    public class ConcretePryoutShear : AnchorageConcreteLimitState
    {
        bool IsGroup;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="h_eff"></param>
        /// <param name="N_cp">For cast-in, expansion, and undercut anchors, Ncp is Ncb determined from Eq. (17.4.2.1a), and for adhesive anchors, Ncp is the lesser of Na determined from Eq. (17.4.5.1a) and Ncb determined from Eq. (17.4.2.1a)</param>
        /// <param name="AnchorType"></param>
        public ConcretePryoutShear
             (  int n, 
                double h_eff,
                double N_cp,
                AnchorInstallationType AnchorType 
            )
            : base(n, h_eff, AnchorType)
            {
                this.N_cp = N_cp;
            }

        double N_cp;

        public override double GetNominalStrength()
        {
            double k_cp = Get_kcp();

            double V_cp = Get_V_cp(k_cp, N_cp);

            return V_cp;
        }

        private double Get_V_cp(double k_cp, double N_cp)
        {
            return k_cp * N_cp; //17.5.3.1a  and 17.5.3.1b
        }

        private double Get_kcp()
        {
            if (h_eff<2.5) //2.5 has units of inches!
            {
                return 1.2; 
            }
            else
            {
                return 2.0;
            }
        }
    }
}
