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
using System.Threading.Tasks;
using Kodestruct.Common.CalculationLogger.Interfaces;
using Kodestruct.Common.Entities;
using Kodestruct.Common.Section.Interfaces;
using Kodestruct.Steel.AISC.AISC360v10.General.Compactness;
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.SteelEntities;


namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public partial class BeamRectangularHss : FlexuralMemberRhsBase, ISteelBeamFlexure
    {
        ICalcLog CalcLog;
        MomentAxis MomentAxis;

        public BeamRectangularHss(ISteelSection section, FlexuralCompressionFiberPosition compressionFiberPosition, MomentAxis MomentAxis, ICalcLog CalcLog)
            : base(section, CalcLog)
        {
            GetSectionValues();
            this.MomentAxis = MomentAxis;
            FlangeCompactnessClass = GetFlangeCompactness(compressionFiberPosition, MomentAxis);

        }


        public CompactnessClassFlexure FlangeCompactness { get; set; }
        public CompactnessClassFlexure WebCompactness { get; set; }

        CompactnessClassFlexure FlangeCompactnessClass;

        //This section applies to square and rectangular HSS, and doubly symmetric boxshaped
        //members bent about either axis, having compact or noncompact webs and
        //compact, noncompact or slender flanges as defined in Section B4.1 for flexure.


        #region Limit States

        public virtual SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            return GetPlasticMomentCapacity(MomentAxis);
        }


        public virtual SteelLimitStateValue GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {

            SteelLimitStateValue ls = new SteelLimitStateValue();
            

            if (FlangeCompactnessClass == CompactnessClassFlexure.Compact)
            {
                ls = new SteelLimitStateValue(-1, false);
            }
            else
            {
                double phiM_n = GetCompressionFlangeLocalBucklingCapacity(CompressionLocation, MomentAxis);
                ls = new SteelLimitStateValue(phiM_n, true);
            }


            return ls;
        }


        public SteelLimitStateValue GetFlexuralWebOrWallBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {

            SteelLimitStateValue ls = new SteelLimitStateValue();

            ShapeCompactness.HollowMember Compactness = new ShapeCompactness.HollowMember(Section, CompressionLocation, MomentAxis);
            CompactnessClassFlexure cClass = Compactness.GetWebCompactnessFlexure();

            if (cClass == CompactnessClassFlexure.Compact)
            {
                ls = new SteelLimitStateValue(-1, false);

            }
            else
            {
                double phiM_n = GetWebLocalBucklingCapacity(MomentAxis, CompressionLocation);
                ls = new SteelLimitStateValue(phiM_n, true);
            }
            return ls;
        }



        #endregion


        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            Fy = Section.Material.YieldStress;

        }

        double E;
        double Fy;

    }
}
