#region Copyright
   /*Copyright (C) 2015 Kodestruct Inc

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
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;

using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.Steel.Entities;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamISlenderWeb : FlexuralMemberIBase
    {
        ISectionI SectionI;

        public BeamISlenderWeb(ISteelSection section, bool IsRolledMember, ICalcLog CalcLog)
            : base(section, IsRolledMember, CalcLog)
        {

            SectionI = this.Section as ISectionI;
            if (section == null)
            {
                throw new SectionWrongTypeException(typeof(ISectionI));
            }
            GetSectionValues();
        }


    //This section applies to doubly symmetric I-shaped members bent about their major
    //axis with noncompact webs and singly symmetric I-shaped members with webs
    //attached to the mid-width of the flanges, bent about their major axis, with compact
    //or noncompact webs, as defined in Section B4.1 for flexure.


        #region Limit States

        public override SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralLateralTorsionalBucklingStrength(double C_b, double L_b, FlexuralCompressionFiberPosition CompressionLocation,
            FlexuralAndTorsionalBracingType BracingType)
        {
            SteelLimitStateValue ls;
            if (BracingType == FlexuralAndTorsionalBracingType.FullLateralBracing)
            {
                ls = new SteelLimitStateValue(-1, false);
            }
            else
            {
                double phiM_n = GetLateralTorsionalBucklingStrength(CompressionLocation,L_b, C_b);
                ls = new SteelLimitStateValue(phiM_n, true);
            }
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetCompressionFlangeLocalBucklingCapacity(CompressionLocation);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralTensionFlangeYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetTensionFlangeYieldingCapacity(CompressionLocation);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
            return ls;
        }

        public override SteelLimitStateValue GetFlexuralCompressionFlangeYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetCompressionFlangeYieldingCapacity(CompressionLocation);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
            return ls;
        }



        public override SteelLimitStateValue GetLimitingLengthForInelasticLTB_Lr(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double r_t = GetEffectiveRadiusOfGyration_r_t(CompressionLocation);
            double L_r = GetLr(r_t);
            SteelLimitStateValue ls = new SteelLimitStateValue(L_r, true);
            return ls;
        }

        public override SteelLimitStateValue GetLimitingLengthForFullYielding_Lp(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double r_t = GetEffectiveRadiusOfGyration_r_t(CompressionLocation);
            double L_p = GetLp(r_t);
            SteelLimitStateValue ls = new SteelLimitStateValue(L_p, true);
            return ls;
        }


        #endregion


        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            Fy = Section.Material.YieldStress;

            Iy = Section.Shape.I_y;

            Sxbot = Section.Shape.S_xBot;
            Sxtop = Section.Shape.S_xTop;

            Sx = Math.Min(Sxbot, Sxtop);

            Zx = Section.Shape.Z_x;


            //Cw = Section.SectionBase.C_w;

            //J = Section.SectionBase.J;

            //c =  Get_c();

            //ho = Get_ho();
        }

        double E;
        double Fy;

        double Iy;

        double Sxbot;
        double Sxtop;

        double Sx;
        double Zx;

        //double Cw;
        //double J;
        //double c;
        //double ho;





    }
}
