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
 using Kodestruct.Common.CalculationLogger;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Common;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Steel.Entities;

 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamAngle : FlexuralMemberAngleBase
    {
        AngleRotation AngleRotation;
        AngleOrientation AngleOrientation;
        MomentAxis MomentAxis;

        public BeamAngle(ISteelSection section, ICalcLog CalcLog, AngleRotation AngleRotation,  MomentAxis MomentAxis, 
            AngleOrientation AngleOrientation= AngleOrientation.ShortLegVertical)
            : base(section, CalcLog, AngleOrientation)
        {
            this.AngleRotation = AngleRotation;
            this.AngleOrientation = AngleOrientation;
            this.MomentAxis = MomentAxis;

            GetSectionValues();

        }



        #region Limit States

        public override SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetYieldingMomentCapacityGeometric(CompressionLocation);
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
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
                double phiM_n = GetFlexuralTorsionalBucklingMomentCapacity(L_b, C_b, CompressionLocation, BracingType, MomentAxis);
                ls = new SteelLimitStateValue(phiM_n, true);
            }
            return ls;
        }


        #endregion



        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            F_y = Section.Material.YieldStress;
            ISectionAngle angle = this.Section.Shape as ISectionAngle;

            if (angle == null)
            {
                throw new Exception("ISectionAngle type is required for this calculation.");
            }

            if (angle.d!=angle.b) //unequal leg 
            {
                   IsEqualLeg = false;
            }
            else
            {
                IsEqualLeg = true;
            }

            b = angle.b;
            d = angle.d;
            t = angle.t;
            r_z=angle.r_z;
            I_z=angle.I_z;
            beta_w=angle.beta_w;

            Compactness = new ShapeCompactness.AngleMember(angle, Section.Material, this.AngleOrientation);
        }

        protected double E        {get; set;}
        protected double F_y       {get; set;}
        protected double b        {get; set;}
        protected double d { get; set; }
        protected double t        {get; set;}
        protected bool IsEqualLeg {get; set;}
        protected double r_z      {get; set;}
        protected double I_z      {get; set;}
        protected double beta_w { get; set; }

        ShapeCompactness.AngleMember Compactness { get; set; }
    }
}
