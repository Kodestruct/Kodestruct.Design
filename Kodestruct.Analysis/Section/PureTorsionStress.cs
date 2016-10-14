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

namespace Kodestruct.Analysis.Section
{
    public partial class SectionStressAnalysis
    {
        /// <summary>
        /// Pure torsion stress in open cross section
        /// </summary>
        /// <param name="G">Shear modulus of elasticity</param>
        /// <param name="t_el">Thickness of element</param>
        /// <param name="theta_1der">First derivative of angle of rotation with respect to z</param>
        /// <returns></returns>
        public double GetPureTorsionStressOpenSection(double G, double t_el, double theta_1der)
        {
            double tau_t = G * t_el * theta_1der; //AISC Design Guide 9 Equation(4.1)
            return tau_t;
        }
    }
}
