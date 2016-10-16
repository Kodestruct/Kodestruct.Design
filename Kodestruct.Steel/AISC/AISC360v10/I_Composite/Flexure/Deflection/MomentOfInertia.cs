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
using Kodestruct.Common.Section.Interfaces;

namespace Kodestruct.Steel.AISC.AISC360v10.Composite
{
    public partial class CompositeBeamSection: AnalyticalElement
    {
        //public double GetLowerBoundMomentOfInertia(double SumQ_n, double CForce)
        public double GetLowerBoundMomentOfInertia(double SumQ_n)
        {
            this.SumQ_n = SumQ_n;
            this.C_Slab = GetCForce();
            double A_s = SteelSection.A;
            double d_3 = SteelSection.y_pBar; // In this program Y_p is always from the bottom fiber 
            double d_1 = Get_d_1();
            double I_s = SteelSection.I_x;
            double Y_ENA = GetNeutralAxisPosition(d_1, d_3,A_s);
            
            // (C-I3-1)
            //double I_LB = I_s + A_s * Math.Pow((Y_ENA - d_3), 2) + ((SumQ_n) / (F_y)) * Math.Pow((2 * d_3 + d_1 - Y_ENA), 2);
            double I_LB = I_s + A_s * Math.Pow((Y_ENA - d_3), 2) + ((C_Slab) / (F_y)) * Math.Pow((2 * d_3 + d_1 - Y_ENA), 2);
            return I_LB;
        }

        private double Get_d_1()
        {
            double d_1;

            double a = C_Slab / (0.85 * f_cPrime * SlabEffectiveWidth);

            if (a>SlabSolidThickness)
            {
                d_1 = SlabSolidThickness / 2+SlabDeckThickness;
            }
            else
            {
                d_1 = (SlabSolidThickness - a / 2) + SlabDeckThickness;
            }
            return d_1;
        }

        private double GetNeutralAxisPosition(double d_1, double d_3,double A_s)
        {
            // (C-I3-2)
            //double Y_ENA = (A_s * d_3 + ((SumQ_n / F_y)) * (2 * d_3 + d_1)) / (A_s + (SumQ_n / F_y));
            double Y_ENA = (A_s * d_3 + ((C_Slab / F_y)) * (2 * d_3 + d_1)) / (A_s + (C_Slab / F_y));
            return Y_ENA;
        }
        
    }
}
