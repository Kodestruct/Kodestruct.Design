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

namespace Kodestruct.Loads.ASCE.ASCE7_10.WindLoads
{
    public partial class WindLocation : AnalyticalElement
    {
        public double GetK2(double x, double Mu, double Lh)
        {
           // x= distance upwind or downwind of crest in Fig. 26.8-1, in ft (m)
            //x: Distance (upwind or downwind) from the crest to the building site,
            //µ: Horizontal attenuation factor. 

            double K2 = (1.0- Math.Abs(x)/(Mu*Lh));
            return K2;
        }
    }
}
