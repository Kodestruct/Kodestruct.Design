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
using Kodestruct.Steel.AISC.SteelEntities;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public  partial class PryingActionElement : SteelDesignElement
    {
        //AISC Night School  Bracing Connections and Related Topics
        //October 13, 2014 Session 3: 
        //Bracing Connection Details and Prying Action
        //Slides 60-62

        double T;
        /// <summary>
        /// Gets minimum required plate thickness.
        /// </summary>
        /// <param name="T">Bolt tension force</param>
        /// <returns></returns>
        public  double GetMinimumThickness(double T) 
        {
            this.T = T;
            double t_min = 0.0;
            if (T>B_bolt)
            {
                throw new Exception("Bolt strength is less than demand. Revise connection.");
            }
            else
	        {
                double beta = GetBetaDesign(B_bolt, T);
                double alpha_prime = 0.0;
                if (beta >=1)
                {
                    alpha_prime = 1.0;
                }
                else
                {
                    double alpha_prime1 = (1 / delta) * ((beta / (1 - beta)));
                    double alpha_prime2 = 1.0;
                    alpha_prime = Math.Min(alpha_prime1, alpha_prime2);
                }

                t_min = t_c * Math.Sqrt(((T / B_bolt)) * ((1 / (1 + delta * alpha_prime))));
	        }
            return t_min;
        }

        /// <summary>
        /// Gets parameter beta for prying action calculations
        /// </summary>
        /// <param name="B">Available tensile strength of bolt (phiF_nt*A_b or phiF_nt' *A_b)  </param>
        /// <param name="T">Bolt tension force</param>
        /// <returns></returns>
        private double GetBetaDesign(double B, double T)
        {
            double beta = 1 / rho * (B / T - 1.0);
            return beta;
        }
    }
}
