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
using Kodestruct.Common.Entities; 
using Kodestruct.Common.Section.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;

 

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public abstract class ColumnDoublySymmetric : ColumnFlexuralAndTorsionalBuckling
    {
        //public ColumnDoublySymmetric(ISteelSection Section, double L_x, double L_y, double K_x, double K_y, ICalcLog CalcLog)
        //    : base(Section,L_x,L_y,K_x,K_y, CalcLog)
             public ColumnDoublySymmetric(ISteelSection Section, double L_x, double L_y, double L_z, ICalcLog CalcLog)
            : base(Section,L_x,L_y,L_z, CalcLog)
        {

        }

        public  double GetTorsionalElasticBucklingStressFe()
        {
            double pi2 = Math.Pow(Math.PI, 2);
            double E = Section.Material.ModulusOfElasticity;
            double Cw = Section.Shape.C_w;
            //double Kz = EffectiveLengthFactorZ;
            double Lz = L_ez;

            //todo: change shear modulus to be the material property
            double G = Section.Material.ShearModulus; //ksi
            double J = Section.Shape.J;
            double Ix = Section.Shape.I_x;
            double Iy = Section.Shape.I_y;

            double Fe;
            //if (Kz * Lz == 0)
            if (Lz == 0)
            {
                double F_y = this.Section.Material.YieldStress;
                return F_y;
            }
            else
            {
                Fe = (pi2 * E * Cw / Math.Pow(Lz, 2) + G * J) * 1 / (Ix + Iy); //(E4-4)
                return Fe;
            }


        }
    }
}
