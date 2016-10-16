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
using Kodestruct.Common.CalculationLogger.Interfaces;

using Kodestruct.Steel.AISC.Entities.Welds.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public class FilletWeld : FilletWeldBase, IWeld
    {
        /// <summary>
        /// Weld full constructor
        /// </summary>
        /// <param name="F_y">Base metal yield stress</param>
        /// <param name="F_u">Base metal tensile stress</param>
        /// <param name="F_EXX">Electrode strength</param>
        /// <param name="Leg"> Weld leg (size)</param>
        /// <param name="A_nBase">Net area of base metal</param>
        /// <param name="Length">Length of weld</param>
        /// <param name="Log">Calculation log</param>
        public FilletWeld(double F_y, double F_u, double F_EXX, double Leg, double A_nBase, double Length, ICalcLog Log)
            : base(F_y, F_u, F_EXX, Leg,A_nBase,Length,  Log)
            {

            }
        public FilletWeld(double F_y, double F_u, double F_EXX, double Leg, double A_nBase, double Length)
            : this(F_y, F_u, F_EXX, Leg, A_nBase, Length,null)
        {

        }

        public double GetStrength(WeldLoadType LoadType, double theta, bool IgnoreBaseMetal)
        {
            double baseMetalStrength;
            if (IgnoreBaseMetal == true)
            {
                baseMetalStrength = double.PositiveInfinity;
            }
            else
            {
                baseMetalStrength = GetBaseMetalShearDesignStress() * A_nBase;
            }

            double weldMetalStrength = GetWeldMetalShearDesignStress(theta)*GetWeldArea();
            return Math.Min(baseMetalStrength, weldMetalStrength);
        }


        public double GetWeldArea()
        {
            return GetEffectiveAreaPerUnitLength() * Length;
        }
    }
}
