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
using Kodestruct.Steel.AISC.Steel.Entities;


namespace Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class RhsTransversePlate : RhsToPlateConnection, IHssTransversePlateConnection
        
    {
        internal SteelLimitStateValue GetHssSideYielding()
            {
            double R=0.0;
            double Rn =0.0;
            
            double beta = GetBeta();
            double Fy = Hss.Material.YieldStress;
            double t = Hss.Section.t_des;
            double lb = Plate.Section.B;

            if (beta == 1.0)
	            {
		            double k = 1.5*t; //outside radius
                    Rn = 2.0*Fy*t*(5.0*k+lb);  //(K1-9)
                    R = Rn * 1.0;
                    return new SteelLimitStateValue(R, false);
	            }
            else
	            {
                    return new SteelLimitStateValue(-1, false);
	            }
  


            }


    }
}
