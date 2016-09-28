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
 
using Kodestruct.Common.Mathematics;
using Kodestruct.Common.CalculationLogger.Interfaces; 
using Kodestruct.Steel.AISC.Interfaces;
using Kodestruct.Steel.AISC.SteelEntities.Sections;



namespace Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public partial class ChsLongitudinalPlate : ChsToPlateConnection, IHssLongitudinalPlateConnection
    {



        double GetHssPlastificationStrength()
        {
            double R = 0.0;
            double Rn = 0.0;

            double theta = this.angle;
            double sinTheta = Math.Sin(theta.ToRadians());

            double Fy = 0.0; double t = 0.0; double Bp = 0.0; double D = 0.0; double tp = 0.0;

            this.GetTypicalParameters(ref Fy, ref t, ref Bp, ref D,ref tp);
            double lb = Bp/sinTheta;



            Rn = (5.5 * Fy * Math.Pow(t, 2) * (1.0 + 0.25 * lb / D) * Q_f)/sinTheta; //(K1-2)

                R = 0.9 * Rn;


            return R;
        }

    }
}
