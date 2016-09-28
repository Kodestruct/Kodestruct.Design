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
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Sections;



namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class RhsTransversePlate: RhsToPlateConnection, IHssTransversePlateConnection
    {


        internal double GetLocalYieldingOfPlate()
        {
            double B =  this.Hss.Section.B;
            double Bp = Plate.Section.H;
            double Fy = Hss.Material.YieldStress;
            double Fyp = Plate.Material.YieldStress;
            double t = this.Hss.Section.t_des;
            double tp = Plate.Section.B;
            double beta = Bp / B;


            //(K1-7)
            double R;
            double Rn = 0.0;
            Rn = 10.0/(B/t)*Fy*Bp;
            double RnMax = Fyp * tp * Bp;
            Rn = Rn > RnMax ? RnMax : Rn;

                R = Rn * 0.95;

            return R;
        }



    }
}
