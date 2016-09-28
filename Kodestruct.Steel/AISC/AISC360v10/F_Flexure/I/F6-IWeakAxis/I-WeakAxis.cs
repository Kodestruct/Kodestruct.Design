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
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.Steel.Entities;
 
 
 

namespace Kodestruct.Steel.AISC.AISC360v10.Flexure
{
    public  partial class BeamIWeakAxis : FlexuralMemberIBase
    {

        public BeamIWeakAxis (ISteelSection section, bool IsRolledMember, ICalcLog CalcLog)
            : base(section, IsRolledMember, CalcLog)
        {
        }


        #region Limit States

        public override SteelLimitStateValue GetFlexuralYieldingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            return GetMinorPlasticMomentStrength();
        }


        public override SteelLimitStateValue GetFlexuralFlangeLocalBucklingStrength(FlexuralCompressionFiberPosition CompressionLocation)
        {
            double phiM_n = GetCompressionFlangeLocalBucklingCapacity();
            SteelLimitStateValue ls = new SteelLimitStateValue(phiM_n, true);
            return ls;
        }




        #endregion

        internal void GetSectionValues()
        {

            E = Section.Material.ModulusOfElasticity;
            Fy = Section.Material.YieldStress;
            Zy = Section.Shape.Z_y;
            Sy = Math.Min(Section.Shape.S_yLeft, Section.Shape.S_yRight);

           }

        double E;
        double Fy;
        double Zy;
        double Sy;


    }
}
