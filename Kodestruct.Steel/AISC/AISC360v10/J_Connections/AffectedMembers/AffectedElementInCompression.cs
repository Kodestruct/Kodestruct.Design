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

using Kodestruct.Steel.AISC.SteelEntities.Materials;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Common.Section.SectionTypes;
using Kodestruct.Steel.AISC.AISC360v10.Compression;

namespace Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public partial class AffectedElementInCompression : AffectedElement
    {
            public AffectedElementInCompression(double F_y, double b,double t)
            {
                SteelMaterial material = new SteelMaterial(F_y, double.PositiveInfinity, SteelConstants.ModulusOfElasticity, SteelConstants.ShearModulus);
                SectionRectangular section = new SectionRectangular(b,t);
                Section = new SteelPlateSection(section, material);
            }

            public double GetCompressionCapacity(double L_e)
            {

             
                    double F_y = Section.Material.YieldStress;
                    double r = Section.Shape.r_y;
                    double phiP_n = 0.0;
                    double KLr = L_e/r;
                    if (KLr <= 25) // per J4.4
	                {
                        double A_g = Section.Shape.A;
                        phiP_n = 0.9* A_g * F_y;  // (J4-6)
	                }
                    else
                    {
                        CompressionMemberRectangle rectangularColumn = new CompressionMemberRectangle(Section,L_e,L_e,L_e); //todo: update CalcLog
                        phiP_n = rectangularColumn.CalculateDesignStrength(false);
                    }

                    return phiP_n;
            }
    }
}
