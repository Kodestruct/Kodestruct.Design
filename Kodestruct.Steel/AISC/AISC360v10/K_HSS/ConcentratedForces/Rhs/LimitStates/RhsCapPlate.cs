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
using Kodestruct.Common.Section.Interfaces;

using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;

namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class RhsCapPlate : RhsToPlateConnection
    {
        public RhsCapPlate(SteelRhsSection Hss, SteelPlateSection Plate, ICalcLog CalcLog, bool IsTensionHss,
            double P_uChord, double M_uChord)
            : base(Hss, Plate, CalcLog, IsTensionHss,P_uChord,M_uChord)
        {

        }

        public double GetAvailableStrength()
        {
            double R = 0.0;
            double RLocalYielding = GetLocalYieldingOnSidewalls();
            double RSideWallCrippling = GetLocalCripplingOfSideWalls();

            R = Math.Min(RLocalYielding, RSideWallCrippling);

            return R;
        }

        internal double GetLocalYieldingOnSidewalls()
        {
            double R = 0;
            
            ISectionTube tube = Hss.Section as ISectionTube;
            if (tube == null)
            {
                throw new Exception("Member must be of type SectionTube");
            }

            double B = tube.B;
            double Fy = Hss.Material.YieldStress;
            double tp = Plate.Section.B;
            double lb = tp; 
            double A = tube.A;

            double Rn = 0.0;
            if ((5.0 * t_plCap + lb) < B)
            {
                //(K1-14a)
                Rn = 2.0 * Fy * t_plCap * (5.0 * t_plCap + lb);
            }
            else
            {
                //(K1-14b)
                Rn = Fy * A;
            }
                R = 1.0 * Rn;
 
            return R;

        }

        internal double GetLocalCripplingOfSideWalls()
        {
            double R = 0.0;
            double Rn = 0.0;
            double tp = Plate.Section.B;
            double lb = tp;
            ISectionTube tube = Hss.Section as ISectionTube;
            if (tube == null)
            {
                throw new Exception("Member must be of type SectionTube");
            }

            double B = tube.B;
            double t = tube.t_des;
            double E = SteelConstants.ModulusOfElasticity;
            double Fy = Hss.Material.YieldStress;

            if (5.0 * t_plCap + lb < B)
            {
                //(K1-15)
                Rn = 1.6 * Math.Pow(t, 2) * (1.0 + 6.0 * lb / B * Math.Pow(t / t_plCap, 1.5)) * Math.Sqrt(E * Fy * t_plCap / t);
            }
            else
            {
                double A = tube.A;
                Rn = Fy * A;
            }
 
                R = 0.75 * Rn;
 

            return R;
        }
    }
}
