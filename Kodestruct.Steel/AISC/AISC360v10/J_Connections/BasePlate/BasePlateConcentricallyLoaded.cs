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
    public class BasePlateConcentricallyLoaded : BasePlateTensionLoaded
    {


        public BasePlateConcentricallyLoaded(IBasePlate Plate)
            :base(Plate)
        {
            this.Plate = Plate;
        }
        public IBasePlate Plate { get; set; }


        public virtual double GetMinimumThicknessConcentricLoad(double P_u)
        {
            double F_y = Plate.F_y;
            double phi_b = 0.9;
            double B = Plate.B_bp;
            double N = Plate.N_bp;
            double l = Plate.GetLength(P_u);
            double t_minimum = l * Math.Sqrt(((2 * P_u) / (phi_b * F_y * B * N)));
            return t_minimum;
        }

        public double GetBearingStrength()
        {
            return Plate.GetphiP_p();
        }
    }
}
