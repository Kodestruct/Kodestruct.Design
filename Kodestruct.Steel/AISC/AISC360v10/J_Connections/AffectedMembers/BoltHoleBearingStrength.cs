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
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Sections;

using Kodestruct.Steel.AISC;
using Kodestruct.Steel.AISC.SteelEntities.Bolts;

namespace Kodestruct.Steel.AISC360v10.Connections.AffectedElements
{
    public  partial class AffectedElementWithHoles : SteelDesignElement
    {
        public AffectedElementWithHoles()
        {

        }
        public AffectedElementWithHoles(ICalcLog log): base(log)
        {

        }
        /// <summary>
        /// Bearing Strength at Bolt Holes
        /// </summary>
        /// <param name="l_c">Clear distance, in the direction of the force, between the edge of the hole and the edge of the adjacent hole or edge of the material</param>
        /// <param name="d_b">Nominal bolt diameter</param>
        /// <param name="t">Thickness of connected material</param>
        /// <param name="F_y">Yield stress of base metal</param>
        /// <param name="F_u">Ultimate stress of base metal</param>
        /// <param name="BoltHoleType">Type of bolt hole</param>
        /// <param name="BoltHoleDeformationType">Identifies whetehr deformation at the bolt hole at service load is a design consideration</param>
        /// <param name="IsUnstiffenedHollowSection">Identifies whether this is a connection made using bolts that pass completely through an unstiffened box member or HSS</param>
        /// <returns></returns>
        public double GetBearingStrengthAtBoltHole(double l_c, double d_b, double t, double F_y, double F_u, BoltHoleType BoltHoleType, 
            BoltHoleDeformationType BoltHoleDeformationType, bool IsUnstiffenedHollowSection=false)
        {
            double phiR_n;
            if (IsUnstiffenedHollowSection == false)
            {
                        double phiR_n1;
                        double phiR_n2;
           
                if (BoltHoleType == Kodestruct.Steel.AISC.SteelEntities.Bolts.BoltHoleType.LSL_Perpendicular)
                {
                        //(J3-6c)
                        phiR_n1=0.75*(1.0*l_c*t*F_u);
                        phiR_n2=0.75*(2.0*d_b*t*F_u);
                }
                else
                {
                    if (BoltHoleDeformationType == Kodestruct.Steel.AISC.BoltHoleDeformationType.ConsideredUnderServiceLoad)
                    {
                         //(J3-6a)
                        phiR_n1=0.75*(1.2*l_c*t*F_u);
                        phiR_n2=0.75*(2.4*d_b*t*F_u);
 
                    }
                    else
                    {
                       //(J3-6b)
                        phiR_n1=0.75*(1.5*l_c*t*F_u);
                        phiR_n2=0.75*(3.0*d_b*t*F_u);
                    }
                }
                phiR_n = Math.Min(phiR_n1, phiR_n2);
            }
            else
	            {
                    phiR_n=0.75*(1.8*d_b*t*F_y);
	            }

            return phiR_n;
            }
        }

    
}
