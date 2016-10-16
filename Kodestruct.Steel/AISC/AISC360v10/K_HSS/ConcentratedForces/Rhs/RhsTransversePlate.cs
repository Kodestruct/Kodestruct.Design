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
using Kodestruct.Steel.AISC.Entities;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;
using Kodestruct.Steel.AISC.SteelEntities.Sections;


namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class RhsTransversePlate: RhsToPlateConnection, IHssTransversePlateConnection
    {

        public RhsTransversePlate(SteelRhsSection Hss, SteelPlateSection Plate, ICalcLog CalcLog, bool IsTensionHss, TransversePlateType PlateType,
            double P_uHss, double M_uHss)
            : base(Hss, Plate, CalcLog, IsTensionHss,P_uHss,M_uHss)
        {
            this.PlateType = PlateType;
        }

        TransversePlateType PlateType;
        public SteelLimitStateValue GetLocalCripplingAndYieldingStrengthOfHss()
        {

            //(K1-9) (K1-10) (K1-11)
            double phiR_n;
            //= GetLocalYieldingOfPlate();
            //double phiR_n1 = 

            SteelLimitStateValue SideYieldingLimitState = GetHssSideYielding();
            SteelLimitStateValue LocalCripplingLimitState;

            if (PlateType == TransversePlateType.TConnection)
            {
                LocalCripplingLimitState = GetLocalCripplingOfSideWallsTee();
            }
            else
            {
                LocalCripplingLimitState = GetLocalCripplingOfSideWallsTee();
            }
            if (SideYieldingLimitState.IsApplicable == true)
            {
                if (SideYieldingLimitState.Value<LocalCripplingLimitState.Value)
                {
                    return SideYieldingLimitState;
                }
                else
                {
                   return LocalCripplingLimitState;
                }
            }
            else
            {
                return LocalCripplingLimitState;
            }

        }

        public SteelLimitStateValue GetLocalPunchingStrengthOfPlate()
        {
            //(K1-8)
            double phiR_n = GetHssPunching();
            return new SteelLimitStateValue(phiR_n, false);
        }

        public SteelLimitStateValue GetLocalYieldingStrengthOfPlate()
        {
            // (K1-7)
            double phiR_n = GetLocalYieldingOfPlate();
            return new SteelLimitStateValue(phiR_n, false);
        }
    }
}
