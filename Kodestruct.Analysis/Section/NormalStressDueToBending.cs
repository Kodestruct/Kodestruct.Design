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
        /// Normal stress due to flexure
        /// </summary>
        /// <param name="M">Bending moment</param>
        /// <param name="y"> distance to fiber from neutral axis</param>
        /// <param name="I">Moment of inertia</param>
        public double GetNormalStressDueToBending(double M, double y, double I )
        {
            double sigma_b = M * y / I; //Mechanics of materials
            return sigma_b;
        }

        /// <summary>
        /// Normal stress due to flexure
        /// </summary>
        /// <param name="M">Bending moment</param>
        /// <param name="S"> Section modulus</param>
        public double GetNormalStressDueToBending(double M, double S)
        {
            double sigma_b = M /S; //Mechanics of materials
            return sigma_b;
        }
    }
}
