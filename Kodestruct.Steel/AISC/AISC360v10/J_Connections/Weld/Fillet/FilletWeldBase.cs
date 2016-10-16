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
using Kodestruct.Common.Mathematics;


namespace Kodestruct.Steel.AISC.AISC360v10.Connections.Weld
{
    public abstract partial class FilletWeldBase : Weld
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="F_y">Base metal yield stress</param>
        /// <param name="F_u">Base metal ultimate stress</param>
        /// <param name="F_EXX">Electrode strength</param>
        /// <param name="Leg">Weld leg size</param>
        /// <param name="Log">Calculation log (for report generation)</param>
        public FilletWeldBase(double F_y, double F_u, double F_EXX, double Leg, double A_nBase,double l, ICalcLog Log)
            : base(F_y, F_u, F_EXX, Leg, A_nBase, l, Log)
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="F_y">Base metal yield stress</param>
        /// <param name="F_u">Base metal ultimate stress</param>
        /// <param name="F_EXX">Electrode strength</param>
        /// <param name="Leg">Weld leg size</param>
        public FilletWeldBase(double F_y, double F_u, double F_EXX, double Leg, double A_nBase, double l)
            : base(F_y, F_u, F_EXX, Leg, A_nBase,l)
        {

        }

        /// <summary>
        /// Weld lize (leg length)
        /// </summary>

        private double b;

        

        /// <summary>
        /// Gets design shear stress per AISC Table J2.5 Available Strength of Welded Joints
        /// </summary>
        /// <param name="typeOfConnection"></param>
        /// <returns></returns>
        protected double GetWeldMetalShearDesignStress(double theta=0.0)
        {
            double phi1 = 0.75;
            double F_EXX = this.WeldMaterial.ElectrodeStrength;
            double F_nw = 0.6 * F_EXX * (1 + 0.5 * Math.Pow(Math.Sin(theta.ToRadians()), 1.5)); // (J2-5)
            double phiR_n1 = phi1 * F_nw;
            return phiR_n1;
        }

        protected double GetBaseMetalShearDesignStress()
        {
            //refer to Tom Murray AISC webinar
            //Fundamentals of connection design
            //July 31, 2013 page 48.
            //Base metal is checked for fracture
            double phi = 0.75;
            double phiR_n = phi * 0.6 * BaseMaterial.UltimateStress;
            return phiR_n;
        }

        /// <summary>
        /// Effective throat area
        /// </summary>
        /// <returns>Effective area through plane 2-2 of weld per commentary J2.4 Figure C-J2.10 </returns>
        public double GetEffectiveAreaPerUnitLength()
        {
            double throat = Size * Math.Cos(Math.PI / 4.0);
            return throat;
        }
    }
}
