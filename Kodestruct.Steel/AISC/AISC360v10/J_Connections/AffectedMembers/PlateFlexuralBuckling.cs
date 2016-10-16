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
using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Flexure;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC360v10.Connections.AffectedElements;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.AffectedMembers
{
    public partial class AffectedElementInFlexure : AffectedElement
    {

        public double GetPlateFlexuralBucklingStrength(double L_plate)
        {
            SectionRectangular r = Section.Shape as SectionRectangular;
            if (r == null)
            {
                throw new Exception("Plate buckling stress not calculated. Need SectionRectangular as Section property ");
            }
            double S = Math.Min(r.S_xBot, r.S_xTop);
            double F_cr = GetPlateBucklingCriticalStress( L_plate);
            return 0.9 * S * F_cr;
        }
        public  double GetPlateBucklingCriticalStress(double L_plate)
        {
            double lambda = GetLambda(L_plate);
            double F_y = this.Section.Material.YieldStress;

            double Q = GetBucklingReductionCoefficientQ(lambda);
            double F_cr = F_y * Q;
            return F_cr;
        }

        public double GetBucklingReductionCoefficientQ(double lambda)
        {
            double Q;
            if (lambda <= 0.7)
            {
                Q = 1.0;
            }
            else if (lambda <= 1.41)
            {
                Q = (1.34 - 0.468 * lambda);
            }
            else
            {
                Q = ((1.3) / (Math.Pow(lambda, 2)));
            }
            return Q;
        }

        public double GetLambda(double L_plate)
        {
            SectionRectangular r = Section.Shape as SectionRectangular;
            if (r == null)
            {
                throw new Exception("Plate buckling stress not calculated. Need SectionRectangular as Section property ");
            }

            double h_o = r.H;
            double t_w =r.B;

            double F_y = this.Section.Material.YieldStress;
            double lamda = ((h_o * Math.Sqrt(F_y)) / (10 * t_w * Math.Sqrt(475 + 280 * Math.Pow((((h_o) / (L_plate))), 2))));
            return lamda;
        }
    }
}
