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
using Kodestruct.Steel.AISC.Steel.Entities;


namespace Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class RhsTransversePlate : RhsToPlateConnection, IHssTransversePlateConnection
    //Note: the difference between RhsTransversePlateTeeAxial and RhsTransversePlateCrossAxial
    //Local Crippling of HSS Sidewalls limit state
    {


        internal SteelLimitStateValue GetLocalCripplingOfSideWallsTee()
       {
           double R = 0.0;
           double Rn = 0.0;

           double H = this.Hss.Section.H;
           double E = this.Hss.Material.ModulusOfElasticity;
           double Fy = Hss.Material.YieldStress;
           double lb = Plate.Section.B;
           double t = this.Hss.Section.t_des;

           double Qf = RhsStressInteractionQf(HssPlateOrientation.Transverse);

           Rn=1.6* Math.Pow(t,2)*(1+3.0*lb/(H-3.0*t))*Math.Sqrt(E*Fy)*Qf;

                R = Rn * 1.0;


           return new SteelLimitStateValue(R,true);
       }

                

    }
}
