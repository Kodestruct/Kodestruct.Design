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
using Kodestruct.Concrete.ACI;
using Kodestruct.Common.Entities;
using Kodestruct.Common.CalculationLogger.Interfaces;

namespace Kodestruct.Concrete.ACI318_14
{
    public partial class DevelopmentTension: Development
    {
        //cb = smaller of: 
        //(a) the distance from center of a
        //bar or wire to nearest concrete surface, and
        
        //(b) one-half the center-to-center spacing of
        //bars or wires being developed, in.,
        public double GetCb()
        {
            double DistFromCenterToSurface = clearCover + Rebar.Diameter / 2.0;
            double HalfOfCenterToCenterDistance = (clearSpacing + Rebar.Diameter)/2.0;
            double cb= Math.Min(DistFromCenterToSurface, HalfOfCenterToCenterDistance);

            
           return cb;
        }
    }
}

