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
        /// Shear stress due to applied shear
        /// </summary>
        /// <param name="V">Internal shear force</param>
        /// <param name="Q">Statical moment for the point in question</param>
        /// <param name="I">Moment of inertia (I_x or I_y where applicable)</param>
        /// <returns></returns>
        public double GetShearStressDueToAppliedShear(double V, double Q, double I, double b)
        {
            double tau_b = V * Q / (I*b);
            return tau_b;
        }
    }
}
