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

using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.Steel.Entities;



namespace Kodestruct.Steel.AISC.SteelEntities.Members
{
    public abstract class SteelAxialMember : SteelMemberBase, ISteelCompressionMember, ISteelTensionMember
    {
        public abstract SteelLimitStateValue GetFlexuralBucklingStrength();
        public abstract SteelLimitStateValue GetTorsionalAndFlexuralTorsionalBucklingStrength(bool EccentricBrace);

        public double L_ex { get; set; }
        public double L_ey { get; set; }
        public double L_ez { get; set; }

        public double CmFactorX { get; set; }
        public double CmFactorY { get; set; }

        private double netArea;

        public double NetArea
        {
            get { return netArea; }
            set { netArea = value; }
        }

            //    public SteelAxialMember(ISteelSection Section, double L_x, double L_y, double K_x, double K_y, ICalcLog CalcLog) //, ISteelMaterial Material)
            //:base(Section,  CalcLog) //,Material)
        public SteelAxialMember(ISteelSection Section, double L_ex, double L_ey,  ICalcLog CalcLog) //, ISteelMaterial Material)
            :base(Section,  CalcLog) //,Material)
        {
            this.L_ex = L_ex;
            this.L_ey = L_ey;
        }

        public abstract double CalculateDesignStrength(bool EccentricBrace);

        public abstract double CalculateCriticalStress(bool EccentricBrace);


    }
}
