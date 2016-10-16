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
using Kodestruct.Steel.AISC.Interfaces;
 

namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public abstract partial class RhsToPlateConnection: HssToPlateConnection
    {


        internal double RhsStressInteractionQf(HssPlateOrientation PlateOrientation)
        {

            double Qf = 0.0;
            double Beta = GetBeta();

            if (IsTensionHss == false)
            {
                if (PlateOrientation == HssPlateOrientation.Transverse)
                {
                    //(K1-16)
                    Qf = 1.3 - 0.4 * U / Beta;
                    Qf = Qf < 1.0 ? Qf : 1.0;
                }
                else
                {
                    //(K1-17)
                    Qf = Math.Sqrt(1.0 - Math.Pow(U, 2));
                } 
            }
            else
            {
                Qf = 1.0;
            }
            return Qf;
        }
    }
}
