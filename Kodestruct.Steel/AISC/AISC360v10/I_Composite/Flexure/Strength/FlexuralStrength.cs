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
        public double GetFlexuralStrength(double SumQ_n)
        {
            this.SumQ_n = SumQ_n;
            double A_s = SteelSection.A;
            double MaximumTForce = A_s * F_y;
            //Equations (C-I3-7) and  (C-I3-8)
            this.C_Slab = GetCForce();

            double SteelSectionHeight = (SteelSection.YMax - SteelSection.YMin);

            double d_1 = Get_d_1(); //distance from the centroid of the compression force, C, in the concrete to the top of the steel section
            double d_2; //distance from the centroid of the compression force in the steel section to the top of the steel section
            double d_3; // distance from Py to the top of the steel section
            double C;
            double P_y;
            if (C_Slab >= MaximumTForce)
            {
                //Section is fully composite
                C = MaximumTForce;
                d_2 = 0;
                DistanceFromTopOfSteelToPNY = 0;
                //d_3 = SteelSectionHeight - SteelSection.y_pBar; // distance from Py to the top of the steel section
                d_3 = SteelSectionHeight / 2.0;
                P_y = MaximumTForce;
            }
            else
            {
                //Section is partially composite
                double C_steel = (MaximumTForce - C_Slab) / 2.0;
                double A_sPrime = C_steel / F_y;
                IMoveableSection compressedSteelSection = SteelSection.GetTopSliceOfArea(A_sPrime);
                DistanceFromTopOfSteelToPNY = compressedSteelSection.YMax - compressedSteelSection.YMin;
                var C_steelCoordinate = compressedSteelSection.GetElasticCentroidCoordinate();
                d_2 = SteelSectionHeight - C_steelCoordinate.Y;
                //C = C_Slab + C_steel;
                C = C_Slab;
                //P_y = MaximumTForce- C_steel;
                P_y = MaximumTForce;
                IMoveableSection reducedTensionSection = SteelSection.GetBottomSliceOfArea(A_s - A_sPrime);
                double Y_tens = reducedTensionSection.GetElasticCentroidCoordinate().Y;
                //d_3 = SteelSectionHeight- Y_tens;
                d_3 = SteelSectionHeight / 2.0;
            }

            double phiM_n = 0.9 * (C * (d_1 + d_2) + P_y * (d_3 - d_2)); // (C-I3-10)
            PNYCalculated = true;
            return phiM_n;
        }


        bool PNYCalculated;

        private double DistanceFromTopOfSteelToPNY { get; set; }

        public double GetDistanceFromTopOfSteelToPNY(double SumQ_n)
        {
            if (PNYCalculated ==false)
            {
                double M_n = GetFlexuralStrength(SumQ_n);

            }
            return DistanceFromTopOfSteelToPNY;
        }
        

        private double GetCForce()
        {
            return Math.Min(SumQ_n, SlabEffectiveWidth * SlabSolidThickness * 0.85 * f_cPrime);
        }

        
    }
}
