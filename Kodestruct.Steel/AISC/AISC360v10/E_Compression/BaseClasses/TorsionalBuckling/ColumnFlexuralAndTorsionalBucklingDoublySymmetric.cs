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


 

namespace Kodestruct.Steel.AISC.AISC360v10.Compression
{
    public abstract class ColumnDoublySymmetric : ColumnFlexuralAndTorsionalBuckling
    {
        //public ColumnDoublySymmetric(ISteelSection Section, double L_x, double L_y, double K_x, double K_y, ICalcLog CalcLog)
        //    : base(Section,L_x,L_y,K_x,K_y, CalcLog)
        public ColumnDoublySymmetric(ISteelSection Section, double L_x, double L_y, double L_z,  ICalcLog CalcLog=null)
       : base(Section, L_x, L_y, L_z, CalcLog)
        {
            //this.IsExcentricallyLaterallyConstrained = IsEccentricallyLaterallyConstrained;
        }
        //public bool IsExcentricallyLaterallyConstrained { get; set; }
        public double GetTorsionalElasticBucklingStressFe(bool IsEccentricallyLaterallyConstrained)
        {
            double pi2 = Math.Pow(Math.PI, 2);
            double E = Section.Material.ModulusOfElasticity;
            double C_w = Section.Shape.C_w;
            //double Kz = EffectiveLengthFactorZ;
            double Lz = L_ez;

            //todo: change shear modulus to be the material property
            double G = Section.Material.ShearModulus; //ksi
            double J = Section.Shape.J;
            double I_x = Section.Shape.I_x;
            double I_y = Section.Shape.I_y;

            double Fe;
            //if (Kz * Lz == 0)
            if (Lz == 0)
            {
                double F_y = this.Section.Material.YieldStress;
                return F_y;
            }
            else
            {
                Fe = (pi2 * E * C_w / Math.Pow(Lz, 2) + G * J) * 1 / (I_x + I_y); //(E4-4) 
                if (IsEccentricallyLaterallyConstrained == false)
                {
                    return Fe;
                }
                else
                {
                    if (Section.Shape is ISectionI)
                    {
                        ISectionI Ishape = Section.Shape as ISectionI;
                        double A_g = Ishape.A;
                        double a = Ishape.d / 2.0;
                        double r_x = Ishape.r_x;
                        double r_y = Ishape.r_y;
                        double P_e = 0.9 * ((Math.Pow(Math.PI, 2) * E * (C_w + I_y * Math.Pow(a, 2)) / (Math.Pow((Lz), 2))) + G * J) / (Math.Pow(r_x, 2) + Math.Pow(r_y, 2) + Math.Pow(a, 2));
                        double F_eCATB = P_e / A_g;
                        return F_eCATB;
                    }
                    else
                    {
                        return Fe;
                    }

                }
                // return Fe;
            }


        }
    }
}
