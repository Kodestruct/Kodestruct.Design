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

namespace Kodestruct.Steel.AISC.AISC360v10.Connections
{
    public partial class SeatedConnection : AnalyticalElement
    {

        public double GetTriangularSeatStiffenerDesignAxialForce(double P_u)
        {
            return P_u * Math.Cos(theta); //AISC SCM (15-9)
        }



        public double GetTriangularSeatStiffenerDesignShear(double P_u)
        {
            return P_u * Math.Sin(theta); //AISC SCM (15-6)
        }

        /// <summary>
        /// Ultimate moment
        /// </summary>
        /// <param name="P_u"> Ultimate shear </param>
        /// <param name="e">Eccentricity from force to face of support for stiffener</param>
        /// <returns></returns>
        public double GetTriangularSeatStiffenerDesignMoment(double P_u, double e)
        {

            double b_prime = Get_b_prime();
            double N_u = GetTriangularSeatStiffenerDesignAxialForce(P_u);
            double M_u = P_u * e - N_u * (((b_prime) / (2))); //AISC SCM (15-8)
            return M_u;
        }

        private double Get_b_prime()
        {
            return a_seat * Math.Sin(theta);
        }

        private double Get_a_prime()
        {
            return a_seat / Math.Cos(theta);
        }
    }
}
