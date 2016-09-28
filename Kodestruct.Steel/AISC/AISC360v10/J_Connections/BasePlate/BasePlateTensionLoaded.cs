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
using System.Threading.Tasks;

namespace Kodestruct.Steel.AISC.AISC360v10.Connections.BasePlate
{
    public class BasePlateTensionLoaded
    {
        public BasePlateTensionLoaded(IBasePlate Plate)
        {
            this.Plate = Plate;
        }
        public IBasePlate Plate { get; set; }


        public double GetMinimumBasePlateBasedOnBoltTension(double T_uAnchor, double x_anchor, double b_effPlate)
        {

            double t_p = 2.11 * Math.Sqrt(((T_uAnchor * x_anchor) / (b_effPlate * Plate.F_y)));
            return t_p;
        }
    }
}
