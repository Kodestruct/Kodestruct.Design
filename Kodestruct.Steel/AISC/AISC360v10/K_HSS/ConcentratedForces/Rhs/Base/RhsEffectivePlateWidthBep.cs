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
using Kodestruct.Common.Section.Interfaces;
//using Kodestruct.Analytics.Section;
 

namespace  Kodestruct.Steel.AISC360v10.HSS.ConcentratedForces
{
    public abstract partial class RhsToPlateConnection
    {
        public double GetEffectivePlateWidth()
        {
            double Bep;
            double Bp = Plate.Section.H;
            double t = Plate.Section.B;
            ISectionTube tube = Hss.Section as ISectionTube;
            if (tube == null)
            {
                throw new Exception("Member must be of type SectionTube");
            }

            double B = tube.B;
            //(K1-18)
            Bep = 10.0 * Bp / (B / t);
            Bep = Bep > Bp ? Bp : Bep;
            return Bep;
        }
    }
}
