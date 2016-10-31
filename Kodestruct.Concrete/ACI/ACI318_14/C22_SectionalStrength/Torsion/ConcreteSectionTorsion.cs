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
using Kodestruct.Common.Entities;
using Kodestruct.Common;
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Mathematics;

namespace Kodestruct.Concrete.ACI318_14
{
    public class ConcreteSectionTorsion: AnalyticalElement
    {
        public ConcreteSectionTorsion(IConcreteTorsionalShape shape)
        {
            this.Shape = shape;
        }
        public IConcreteTorsionalShape Shape { get; set; }

        public double GetRequiredTorsionTransverseReinforcementArea(double T_u,double s, double f_yt, 
            double theta=45.0)
        {
            double A_o = Shape.GetA_oh();
            double thetaRad = theta.ToRadians();
            double phi = 0.75;
            //(22.7.6.1a)
            double A_tReq = ((T_u * s) / (phi * 2 * A_o * f_yt)) * 1.0 / (Math.Tan(thetaRad));
            return A_tReq;
        }

        
        public double GetRequiredTorsionLongitudinalReinforcementArea(double T_u, double f_y,
    double theta = 45.0)
        {
            double A_o = Shape.GetA_oh();
            double p_h = Shape.Get_p_h();
            double thetaRad = theta.ToRadians();
            double phi = 0.75;
            //(22.7.6.1b)
            double A_lReq = ((T_u * p_h) / (phi * 2 * A_o * f_y)) * 1.0 / (Math.Tan(thetaRad));
            return A_lReq;
        }

        public double GetThreshholdTorsion(double N_u)
        {
            return Shape.Get_T_th(N_u);
        }

        public double GetMaximumForceInteractionRatio(double V_u, double T_u, double V_c, double b, double d)
        {
            double Sqrt_f_c = Shape.Material.Sqrt_f_c_prime;
            double p_h = Shape.Get_p_h();
            double A_oh = Shape.GetA_oh();
            double IR1 = Math.Sqrt(Math.Pow((((V_u) / (b * d))), 2) + Math.Pow((((T_u * p_h)) / (1.7 * Math.Pow(A_oh, 2))), 2));
            double IR2 = ((V_c) / (b * d)) + 8 * Sqrt_f_c;
            double IR = Math.Max(IR1, IR2);

            return IR;
        }
    }
}
