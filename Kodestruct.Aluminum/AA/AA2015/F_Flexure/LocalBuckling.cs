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
using Kodestruct.Aluminum.AA.AA2015.DesignRequirements.LocalBuckling;
using Kodestruct.Aluminum.AA.Entities;
using Kodestruct.Aluminum.AA.Entities.Enums;
using Kodestruct.Aluminum.AA.Entities.Exceptions;
using Kodestruct.Aluminum.AA.Entities.Interfaces;
using Kodestruct.Aluminum.AA.Entities.Section;
using Kodestruct.Common.Section.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodestruct.Aluminum.AA.AA2015.Flexure
{
    public partial class AluminumFlexuralMember : IAluminumBeamFlexure
    {
        //Direct Strength Method F.3-2

        public AluminumLimitStateValue GetLocalBucklingStrength(double b, double t, LateralSupportType LateralSupportType,
        FlexuralCompressionFiberPosition CompressionLocation, WeldCase WeldCase,
        SubElementType SubElementType = SubElementType.Flat)
        {
            double S_xc = GetSectionModulusCompressionSxc(CompressionLocation);
            double M_np = this.GetPlasticMoment();
            FlexuralLocalBucklingElement LocalElement = new FlexuralLocalBucklingElement(this.Section.Material, b, t, LateralSupportType,
                M_np, S_xc, WeldCase, SubElementType);

            double F_b = LocalElement.GetCriticalStress();
            double M_n= S_xc * F_b;
            AluminumLimitStateValue Y = GetFlexuralYieldingStrength(CompressionLocation);

            bool applicable;
            double val;

            if (M_n > Y.Value)
            {
                applicable = false;
                val = -1.0;
            }
            else
            {
                applicable = true;
                val = 0.9*M_n;
            }

            return new AluminumLimitStateValue(val, applicable);
        }

        public AluminumLimitStateValue GetLocalBucklingFlexuralCriticalStress(double b, double t, LateralSupportType LateralSupportType,
        FlexuralCompressionFiberPosition CompressionLocation, WeldCase WeldCase,
        SubElementType SubElementType = SubElementType.Flat)
        {
            double S_xc = GetSectionModulusCompressionSxc(CompressionLocation);
            double M_np = this.GetPlasticMoment();
            FlexuralLocalBucklingElement LocalElement = new FlexuralLocalBucklingElement(this.Section.Material, b, t, LateralSupportType,
                M_np, S_xc, WeldCase, SubElementType);

            double F_b = LocalElement.GetCriticalStress();
            double F_y = 0;
            if (WeldCase == Entities.WeldCase.NotAffected)
            {
                F_y = this.Section.Material.F_ty;

            }
            else
            {
                F_y = this.Section.Material.F_tyw;
            }


            if (F_b>F_y)
            {
                F_b = F_y;
            }

            return new AluminumLimitStateValue(F_b, true);
        }
    }
}
