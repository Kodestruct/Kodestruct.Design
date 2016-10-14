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
            F_y = Section.Material.YieldStress;
            Z_y = Section.Shape.Z_y;
            S_y = Math.Min(Section.Shape.S_yLeft, Section.Shape.S_yRight);

           }


        private double _E;

        public double E
        {
            get {
                GetSectionValues();
                return _E; }
            set { _E = value; }
        }

        private double _F_y;

        public double F_y
        {
            get {
                GetSectionValues();
                return _F_y; }
            set { _F_y = value; }
        }

        private double _Z_y;

        public double Z_y
        {
            get {
                GetSectionValues();
                return _Z_y; }
            set { _Z_y = value; }
        }

        private double _S_y;

        public double S_y
        {
            get {
                GetSectionValues();
                return _S_y; }
            set { _S_y = value; }
        }
        

    }
}
