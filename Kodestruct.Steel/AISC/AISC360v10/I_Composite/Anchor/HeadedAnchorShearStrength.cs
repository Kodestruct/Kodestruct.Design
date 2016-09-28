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
using Kodestruct.Common.Entities;

namespace Kodestruct.Steel.AISC.AISC360v10.Composite
{
    public partial class HeadedAnchor : AnalyticalElement
    {
        public HeadedAnchor(ICalcLog Log): base(Log)
        {

        }
        public HeadedAnchor()
        {
           
        }

        bool StrengthWasCalculated;

        /// <summary>
        /// Nominal (unreduced) shear strength of headed anchor
        /// </summary>
        /// <param name="d_sa">Anchor diameter</param>
        /// <param name="R_g">Group factor</param>
        /// <param name="R_p">Position factor</param>
        /// <param name="fc_prime">concrete specified compressive strength (ksi)</param>
        /// <param name="F_u">Tensile strength of anchor</param>
        /// <param name="w_c">Concrete density (pcf) </param>
        /// <returns></returns>
        public double GetNominalShearStrength(double d_sa,double R_g,double R_p,double fc_prime, double F_u, double w_c)
        {
            double E_c = GetConcreteModulusOfElsticity(w_c, fc_prime);
            double A_sa = GetAnchorArea(d_sa);
            double Q_n1 = 0.5 * A_sa * Math.Sqrt(fc_prime * E_c); //(I8-1)
            double Q_n2 = R_g * R_p * A_sa * F_u; //(I8-1)
            double Q_n = Math.Min(Q_n1, Q_n2);
            return Q_n;
        }

        protected virtual double GetAnchorArea(double d_sa)
        {
            double A_sa = Math.PI * Math.Pow(d_sa, 2) / 4;
            return A_sa;
        }

        private double GetConcreteModulusOfElsticity(double w_c, double fc_prime)
        {
            double E_c = Math.Pow(w_c, 1.5) * Math.Sqrt(fc_prime);
            return E_c;
        }

        /// <summary>
        /// Nominal (unreduced) shear strength of headed anchor
        /// </summary>
        /// <param name="d_sa">Anchor diameter</param>
        /// <param name="R_g">Group factor</param>
        /// <param name="R_p">Position factor</param>
        /// <param name="fc_prime">concrete specified compressive strength (ksi)</param>
        /// <param name="F_u">Tensile strength of anchor</param>
        /// <param name="w_c">Concrete density (pcf) </param>
        /// <returns></returns>
        public double GetShearStrength(double d_sa,double R_g,double R_p,double fc_prime, double F_u, double w_c)
        {
            double Q_n = this.GetNominalShearStrength(d_sa, R_g, R_p, fc_prime, F_u,w_c);
            return 0.65*Q_n; //Page 16.1–103 of the AISC spec
        }

        
        public double GetNominalShearStrength(DeckAtBeamCondition HeadedAnchorDeckCondition,HeadedAnchorWeldCase HeadedAnchorWeldCase, 
            double N_saRib,double e_mid_ht,double h_r,double w_r,double d_sa, double fc_prime, double F_u, double w_c)
        {
            double R_g = GetGroupFactorR_g(HeadedAnchorDeckCondition, HeadedAnchorWeldCase, N_saRib, h_r, w_r);
            double R_p = GetPlacementFactorR_p(HeadedAnchorDeckCondition, HeadedAnchorWeldCase,e_mid_ht);
            return GetNominalShearStrength(d_sa, R_g, R_p, fc_prime, F_u,w_c);
        }
    }
}
