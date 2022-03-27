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

using Kodestruct.Common.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.UFM
{
    /// <summary>
    /// Base plate UFM case 
    /// </summary>
    public class UFMBasePlate : UFMCase
    {
        public UFMBasePlate(double d_b, double d_c, double theta,
            double beta_bar, double P_u, double e)
            : base(d_b, d_c, theta)
        {
            this.beta_bar = beta_bar;
            this.P_u = P_u;
            this.e = e;
            CalculateBasicParameters();
        }

        public double P_u { get; set; }
        public double beta_bar { get; set; }
        public double V { get; set; }
        public double H { get; set; }
        /// <summary>
        /// Eccentricity to workpoint from top of base plate
        /// </summary>
        public double e { get; set; }

        /// <summary>
        /// Base plate horizontal force from gusset weld
        /// </summary>
        public double H_b { get; set; }
        /// <summary>
        /// Horizontal force in column
        /// </summary>
        public double H_c { get; set; }

        /// <summary>
        /// Vertical force in column-gusset interface
        /// </summary>
        public double V_c => V;

        private void CalculateBasicParameters()
        {
            double thetaRad = theta.ToRadians();
            this.H = Math.Sin(thetaRad) * P_u;
            this.V = Math.Cos(thetaRad) * P_u;
            H_b = (H * (beta_bar - e) - V * e_c) / beta_bar;
            H_c = (H * e - V * e_c) / beta_bar;
        }


    }
}

