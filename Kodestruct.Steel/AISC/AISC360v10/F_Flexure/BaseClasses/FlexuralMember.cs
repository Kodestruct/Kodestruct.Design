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
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.SteelEntities.Members;

using Kodestruct.Steel.AISC.Exceptions;
using Kodestruct.Steel.AISC.Steel.Entities;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public abstract partial class FlexuralMember : SteelFlexuralMember, ISteelBeamFlexure
    {
        public FlexuralMember(ISteelSection section, ICalcLog CalcLog)
            : base(section, CalcLog)
        {

        }

        #region Plastic Moment


        public virtual double GetMajorNominalPlasticMoment()
        {
            //this method is used for cases when Full yielding is not an
            //applicable limit state but the Plastic moment
            //Mp is used in calculations
            //In other cases this method is overriden

            double Fy = this.Section.Material.YieldStress;
            double Zx = Section.Shape.Z_x;
            double Mp = Fy * Zx;

            return Mp;
        }

        public virtual double GetMinorNominalPlasticMoment()
        {
            //applicable limit state but the Plastic moment
            //Mp is used in calculations
            //In other cases this method is overriden
            double Fy = this.Section.Material.YieldStress;
            double Zy = Section.Shape.Z_y;
            double Mp = Fy * Zy;

            return Mp;
        }

        #endregion

        #region Yield moment
        internal double GetCompressionFiberYieldMomentMyc(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Myc = 0.0;
            double Sxc = 0.0;

            double Fy = Section.Material.YieldStress;


            Sxc = GetSectionModulusCompressionSxc( compressionFiberPosition);

            Myc = Fy * Sxc;
            return Myc;
        }

        internal double GetTensionFiberYieldMomentMyt(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Myt = 0.0;
            double Sxt = 0.0;


            double Fy = Section.Material.YieldStress;

            Sxt = GetSectionModulusTensionSxt(compressionFiberPosition);

            Myt = Fy * Sxt;
            return Myt;
        }


        public double GetSectionModulusCompressionSxc(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Sxc = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    Sxc = Section.Shape.S_xTop;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    Sxc = Section.Shape.S_xBot;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            return Sxc;
        }

        public double GetSectionModulusTensionSxt(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Sxt = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Top:
                    Sxt = Section.Shape.S_xBot;
                    break;
                case FlexuralCompressionFiberPosition.Bottom:
                    Sxt = Section.Shape.S_xTop;
                    break;
                default:
                    throw new CompressionFiberPositionException();
            }

            return Sxt;
        }

        public double GetSectionModulusCompressionSyc(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Syc = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Left:
                    Syc = Section.Shape.S_yLeft;
                    break;
                case FlexuralCompressionFiberPosition.Right:
                    Syc = Section.Shape.S_yRight;
                    break;
                default:
                    throw new CompressionFiberPositionException();

            }

            return Syc;
        }

        public double GetSectionModulusTensionSyt(FlexuralCompressionFiberPosition compressionFiberPosition)
        {
            double Syc = 0.0;

            switch (compressionFiberPosition)
            {
                case FlexuralCompressionFiberPosition.Left:
                    Syc = Section.Shape.S_yRight;
                    break;
                case FlexuralCompressionFiberPosition.Right:
                    Syc = Section.Shape.S_yLeft;
                    break;
                default:
                    throw new CompressionFiberPositionException();

            }

            return Syc;
        }

        #endregion


        #region Compactness Parameter V E/Fy

        protected double SqrtE_Fy()
        {
            ISteelMaterial material = Section.Material;
            double Fy = material.YieldStress;
            double E = material.ModulusOfElasticity;

            if (Fy == 0 || E == 0)
            {
                throw new Exception("Material Fy or E cannot be zero");
            }
            return Math.Sqrt(E / Fy);
        }

        #endregion

        #region Limit States

        public virtual SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetFlexuralLateralTorsionalBucklingStrength(double C_b, double L_b, FlexuralCompressionFiberPosition CompressionLocation,
            FlexuralAndTorsionalBracingType BracingType)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetFlexuralTensionFlangeYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetFlexuralCompressionFlangeYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetFlexuralWebOrWallBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetFlexuralLegOrStemBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation, 
            FlexuralAndTorsionalBracingType BracingType)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetLimitingLengthForInelasticLTB_Lr(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }

        public virtual SteelLimitStateValue GetLimitingLengthForFullYielding_Lp(FlexuralCompressionFiberPosition CompressionLocation)
        {
            SteelLimitStateValue ls = new SteelLimitStateValue(-1, false);
            return ls;
        }


        #endregion





    }
}
