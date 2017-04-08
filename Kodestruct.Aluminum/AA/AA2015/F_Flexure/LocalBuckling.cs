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
                val = M_n;
            }

            return new AluminumLimitStateValue(val, applicable);
        }
    }
}
