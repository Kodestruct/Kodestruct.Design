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
using Kodestruct.Steel.AISC.Interfaces;
 
 

namespace  Kodestruct.Steel.AISC.AISC360v10.HSS.TrussConnections
{
    public partial class ChsTrussConnection : HssTrussConnection
    {
       public double  GetChordStressInteractionFactorQf(bool ConnectingSurfaceInTension,double RequiredAxialStrenghPro, double RequiredMomentStrengthMro)
        {
            double Qf = 0.0;
            if (ConnectingSurfaceInTension ==true)
            {
                //(K1-5a)
                Qf = 1.0;
            }
            else
            {
                ISteelSection s = Chord.Section;  
                double U = GetUtilizationRatio(s,RequiredAxialStrenghPro, RequiredMomentStrengthMro);
                //(K1-5b)
               Qf = 1.0-0.3*U*(1.0+U);
            }

            return Qf;
        }
    }
}
