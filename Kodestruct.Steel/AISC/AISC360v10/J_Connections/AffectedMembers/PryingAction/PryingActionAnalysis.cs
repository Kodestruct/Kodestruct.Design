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
using Kodestruct.Steel.AISC.SteelEntities;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public  partial class PryingActionElement : SteelDesignElement
    {
        //AISC Night School  Bracing Connections and Related Topics
        //October 13, 2014 Session 3: 
        //Bracing Connection Details and Prying Action
        //Slides 57-59

        public  double GetMaximumBoltTensionForce( double t)
        {
            this.t = t;
            double T_avail = 0.0;
            double alpha_prime = get_alpha_PrimeAnalysis();
            if (alpha_prime<0)
            {
                //The bolts control
                T_avail = B_bolt;
            }
            else if (alpha_prime<=1)
            {
                //The bolts and the fitting both control
                T_avail = B_bolt * Math.Pow(((t / t_c)), 2) * (1 + delta * alpha_prime);
            }
            else
            {
                //The fitting controls
                T_avail = B_bolt * Math.Pow(((t / t_c)), 2) * (1 + delta);
            }
            return T_avail;
        }

        private double get_alpha_PrimeAnalysis()
        {
            double alpha_Prime = ((1) / (delta * (1 + rho))) * (Math.Pow(((t_c / t)), 2) - 1);
            return alpha_Prime;
        }
    }
}
