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
using Kodestruct.Steel.AISC.SteelEntities.Sections;
using Kodestruct.Steel.AISC.SteelEntities;
using Kodestruct.Steel.AISC.Steel.Entities;
using Kodestruct.Steel.AISC.Steel.Entities.Sections;


namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class RhsLongitudinalThroughPlate: RhsToPlateConnection, IHssLongitudinalPlateConnection
    {
        public RhsLongitudinalThroughPlate(SteelRhsSection Hss, SteelPlateSection Plate, ICalcLog CalcLog, bool IsTensionHss, double Angle,
            double P_uHss, double M_uHss)
            : base(Hss, Plate, CalcLog, IsTensionHss,P_uHss,M_uHss)
        {
            this.Angle = Angle;
        }

        private double angle;

        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }


        public SteelLimitStateValue GetHssWallPlastificationStrengthUnderAxialLoad()
        {
            double phiR_n = GetHSSWallPlastification();
            return new SteelLimitStateValue(phiR_n, false);
        }

        public SteelLimitStateValue GetHssMaximumPlateThicknessForShearLoad()
        {
            return new SteelLimitStateValue(-1, false);
        }
    }
}
